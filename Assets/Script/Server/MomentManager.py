import json
from typing import List
from typing import Dict
from Moment import Moment
import json
import copy

path = "uploads/data.json"


class MomentManager:
    """
    Class representing a Moment Manager.

    Attributes:
        json_data (List[Moment]): A list of Moment objects.
        user_json_data (Dict[str, List[int]]): A dictionary mapping usernames to a list of moment indices.
        current_id (int): The current ID for moments.

    Methods:
        __init__(self): Initializes the MomentManager object.
        get_total_photosize_by_name(username: str) -> int: Returns the total photo size for a given username.
        add_moment(data: Moment): Adds a new moment to the manager.
        delete_moment_by_index(idx: int) -> tuple: Deletes a moment by index and returns the name and photo list.
        delete_moment_by_user_index(user: str, idx: int) -> None: Deletes a moment by user index.
        remove_user(user: str) -> None: Removes a user from the manager.
        get_moment_by_index(idx: int) -> str: Returns the moment data as a JSON string by index.
        get_moment_by_user_index(username: str, idx: int) -> str: Returns the moment data as a JSON string by user index.
        save_json_data(): Saves the JSON data to a file.
    """
    json_data: List[Moment] = []
    user_json_data: Dict[str, List[int]] = {}
    current_id: int = 0

    def __init__(self) -> None:
        """
        Initializes the MomentManager object.
        Reads data from a file and adds moments to the manager.
        """
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
        Returns the total photo size for a given username.

        Args:
            username (str): The username.

        Returns:
            int: The total photo size.
        """
        total_size = 0
        if username not in MomentManager.user_json_data:
            MomentManager.user_json_data[username] = []
            return 0

        print(MomentManager.user_json_data[username])
        try:
            for idx in MomentManager.user_json_data[username]:
                total_size += len(MomentManager.json_data[idx].photo_list)
        except IndexError:
            return -1

        return total_size

    @staticmethod
    def add_moment(data: Moment):
        """
        Adds a new moment to the manager.

        Args:
            data (Moment): The moment data to be added.
        """
        data.start_photo_id = MomentManager.current_id
        username = data.name

        if username not in MomentManager.user_json_data:
            MomentManager.user_json_data[username] = []

        MomentManager.user_json_data[username].append(MomentManager.current_id)
        MomentManager.current_id += 1
        MomentManager.json_data.append(data)

    @staticmethod
    def delete_moment_by_index(idx: int) -> tuple:
        """
        Deletes a moment by index and returns the name and photo list.

        Args:
            idx (int): The index of the moment to be deleted.

        Returns:
            tuple: A tuple containing the name and photo list of the deleted moment.
        """
        if idx >= len(MomentManager.json_data):
            return None, []

        photo_list = copy.deepcopy(MomentManager.json_data[idx].photo_list)
        name = copy.deepcopy(MomentManager.json_data[idx].name)
        ridx = MomentManager.json_data[idx].start_photo_id
        MomentManager.user_json_data[name].remove(ridx)
        MomentManager.json_data.pop(idx)

        return name, photo_list

    @staticmethod
    def delete_moment_by_user_index(user: str, idx: int) -> None:
        """
        Deletes a moment by user index.

        Args:
            user (str): The username.
            idx (int): The user index of the moment to be deleted.
        """
        if idx >= len(MomentManager.user_json_data[user]):
            return

        ridx = MomentManager.user_json_data[user][idx]
        MomentManager.user_json_data[user].pop(idx)
        MomentManager.json_data.pop(ridx)

    @staticmethod
    def remove_user(user: str) -> None:
        """
        Removes a user from the manager.

        Args:
            user (str): The username.
        """
        if user not in MomentManager.user_json_data:
            return

    @staticmethod
    def get_moment_by_index(idx: int) -> str:
        """
        Returns the moment data as a JSON string by index.

        Args:
            idx (int): The index of the moment.

        Returns:
            str: The moment data as a JSON string.
        """
        data = MomentManager.json_data[idx].__dict__
        return json.dumps(data)

    @staticmethod
    def get_moment_by_user_index(username: str, idx: int) -> str:
        """
        Returns the moment data as a JSON string by user index.

        Args:
            username (str): The username.
            idx (int): The user index of the moment.

        Returns:
            str: The moment data as a JSON string.
        """
        idx = MomentManager.user_json_data[username][idx]
        return MomentManager.get_moment_by_index(idx)

    @staticmethod
    def save_json_data():
        """
        Saves the JSON data to a file.
        """
        data = []
        for items in MomentManager.json_data:
            data.append(items.__dict__)
        with open(path, "w") as f:
            json.dump(data, f)
