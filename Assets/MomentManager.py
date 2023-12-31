import json
from typing import List
from Moment import Moment
import json

path = "uploads/data.json"


class MomentManager:
    # 静态方法，读取data.json文件，写入一个新数据
    @staticmethod
    def add_moment(data: Moment):
        with open(path, "r") as f:
            moments: List[Moment] = json.load(f)

        data = data.__dict__
        moments.append(data)

        with open(path, "w") as f:
            json.dump(moments, f)

    @staticmethod
    def clear_moment_by_name(name: str):
        with open(path, "r") as f:
            moments: List[Moment] = json.load(f)

        for moment in moments:
            if moment["name"] == name:
                moments.remove(moment)

        with open(path, "w") as f:
            json.dump(moments, f)


MomentManager.add_moment(Moment("nihao", ["1/1.jpg", "1/2.jpg"]))
