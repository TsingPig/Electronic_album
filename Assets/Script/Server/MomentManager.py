import json
from typing import List
from typing import Dict
from Moment import Moment
import json
import copy

path = "uploads/data.json"


class MomentManager:
    json_data: List[Moment] = []
    user_json_data: Dict[str, List[int]] = {}
    current_id: int = 0

    def __init__(self) -> None:
        MomentManager.json_data = []
        MomentManager.user_json_data = {}
        MomentManager.current_id = 0
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
        data.start_photo_id = MomentManager.current_id
        username = data.name

        if username not in MomentManager.user_json_data:
            MomentManager.user_json_data[username] = []

        MomentManager.user_json_data[username].append(MomentManager.current_id)
        MomentManager.current_id += 1
        MomentManager.json_data.append(data)

    @staticmethod
    def delete_moment_by_index(idx: int) -> tuple(str, List[tuple]):
        if idx >= len(MomentManager.json_data):
            return None
        
        photo_list = copy.deepcopy(MomentManager.json_data[idx].photo_list)
        name = copy.deepcopy(MomentManager.json_data[idx].name)
        ridx = MomentManager.json_data[idx].start_photo_id
        MomentManager.user_json_data[name].remove(ridx)
        MomentManager.json_data.pop(idx)

        return name, photo_list


    @staticmethod
    def delete_moment_by_user_index(user: str, idx: int) -> None:
        if idx >= len(MomentManager.user_json_data[user]):
            return

        ridx = MomentManager.user_json_data[user][idx]
        MomentManager.user_json_data[user].pop(idx)
        MomentManager.json_data.pop(ridx) 

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
