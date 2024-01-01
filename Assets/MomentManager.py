import json
from typing import List
from typing import Dict
from Moment import Moment
import json

path = "uploads/data.json"


class MomentManager:
    json_data: List[Moment] = []
    user_json_data: Dict[str, List[int]] = {}

    def __init__(self) -> None:
        with open(path, "r") as f:
            data = json.load(f)
        for items in data:
            MomentManager.add_moment(
                Moment(
                    items["name"],
                    items["photo_list"],
                    items["text"],
                    items["comment_list"],
                )
            )

    @staticmethod
    def get_startphotoid() -> int:
        if len(MomentManager.json_data) == 0:
            return 0
        moment: Moment = MomentManager.json_data[-1]

        start_photo_id = moment.start_photo_id
        size = len(moment.photo_list)
        return start_photo_id + size

    @staticmethod
    def get_total_photosize_by_name(username: str) -> int:
        """
        Calculate the total size of photos for a given username.

        Args:
            username (str): The username to calculate the total photo size for.

        Returns:
            int: The total size of photos for the given username.
        """
        total_size = 0
        if username not in MomentManager.user_json_data:
            MomentManager.user_json_data[username] = []
            return 0

        for idx in MomentManager.user_json_data[username]:
            total_size += len(MomentManager.json_data[idx].photo_list)
        return total_size

    # 静态方法，读取data.json文件，写入一个新数据
    @staticmethod
    def add_moment(data: Moment):
        data.start_photo_id = MomentManager.get_startphotoid()
        username = data.name

        n = len(MomentManager.json_data)
        if username not in MomentManager.user_json_data:
            MomentManager.user_json_data[username] = []

        MomentManager.user_json_data[username].append(n)
        MomentManager.json_data.append(data)

    @staticmethod
    def get_moment_by_index(idx: int) -> str:
        data = MomentManager.json_data[idx].__dict__
        return json.dumps(data)

    @staticmethod
    def get_moment_by_user_index(username: str, idx: int) -> str:
        idx = MomentManager.user_json_data[username][idx]
        return MomentManager.get_moment_by_index(idx)

    @staticmethod
    def save_json_data():
        data = []
        for items in MomentManager.json_data:
            data.append(items.__dict__)
        with open(path, "w") as f:
            json.dump(data, f)
