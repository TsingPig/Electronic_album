import json
from typing import List

class Moment:
    def __init__(
        self,
        name: str,
        photo_list: List[str],
        text: str = None,
        comment_list: List[str] = None,
    ):
        self.name = name
        self.photo_list = photo_list
        self.text = text
        self.comment_list = comment_list

