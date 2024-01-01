from flask import Flask, request, send_file
from werkzeug.utils import secure_filename
from MomentManager import MomentManager
from Moment import Moment
import os
import json
import time

app = Flask(__name__)
app.config["UPLOAD_FOLDER"] = "uploads"
app.config["MAX_CONTENT_LENGTH"] = 16 * 1024 * 1024  # 设置最大上传文件大小为16MB
port = 80
default_path = "uploads/default.png"


# 处理文件上传
@app.route("/upload", methods=["POST"])
def upload_file():
    if "file" not in request.files:
        return "No file part", 400
    file = request.files["file"]
    if file.filename == "":
        return "No selected file", 400
    if file:
        filename = secure_filename(file.filename)
        account = request.form["account"]
        upload_path = os.path.join(app.config["UPLOAD_FOLDER"], account)
        if not os.path.exists(upload_path):
            os.makedirs(upload_path)
        file.save(os.path.join(upload_path, filename))
        return "File uploaded!"


@app.route("/upload_photo", methods=["POST"])
def upload_photo():
    if "file" not in request.files:
        return "No file part", 400
    file = request.files["file"]
    if file.filename == "":
        return "No selected file", 400
    if file:
        # filename = secure_filename(file.filename)
        suffix = file.filename.split(".")[-1]

        if suffix not in ["jpg", "jpeg", "png", "gif", "bmp"]:
            return "File type not supported", 400

        t = time.localtime()
        msec = int(time.time() * 1000) % 1000

        filename = f"{t.tm_year}{t.tm_mon:02}{t.tm_mday:02}_{t.tm_hour:02}{t.tm_min:02}{t.tm_sec:02}_{msec:03}.{suffix}"

        filename = secure_filename(filename)
        account = request.form["account"]
        album_name = request.form["album_name"]
        upload_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
        print(upload_path)
        if not os.path.exists(upload_path):
            os.makedirs(upload_path)
        file.save(os.path.join(upload_path, filename))
        return "File uploaded!"


# 创建相册
@app.route("/createEmptyFolder/<account>/<album_name>", methods=["POST"])
def create_album(account, album_name):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
    if not os.path.exists(album_path):
        os.makedirs(album_path)
        return "Album created successfully!"
    else:
        return "Album already exists."


# 处理文件下载
@app.route("/download/<account>/<filename>", methods=["GET"])
def download_file(account, filename):
    file_path = os.path.join(app.config["UPLOAD_FOLDER"], account, filename)
    if os.path.exists(file_path):
        return send_file(file_path)
    else:
        return "File not found", 404


# 输入用户名，返回对应用户名下的相册列表
@app.route("/get_folders/<account>", methods=["GET"])
def get_folders(account):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account)
    folders = {"folders": []}
    if os.path.exists(album_path):
        # 遍历用户目录下的所有文件夹
        for folder in os.listdir(album_path):
            # 如果是文件夹，将其加入folders字典中
            if os.path.isdir(os.path.join(album_path, folder)):
                folders["folders"].append(folder)

        return json.dumps(folders)
    else:
        return json.dumps(folders)


# 输入用户名和相册名，删除对应相册
@app.route("/delete_folder/<account>/<album_name>", methods=["GET"])
def delete_folder(account, album_name):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
    if os.path.exists(album_path):
        # 删除album_path和其目录里的所有文件
        for photo in os.listdir(album_path):
            os.remove(os.path.join(album_path, photo))
        os.rmdir(album_path)
        return "Album deleted"
    else:
        return "Album not found", 404


# 输入用户名和相册名，返回对应相册下的图片数量
@app.route("/connect_size/<account>/<album_name>", methods=["GET"])
def connect_size(account, album_name):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
    if os.path.exists(album_path):
        photo_size = 0
        for photo in os.listdir(album_path):
            if os.path.isfile(os.path.join(album_path, photo)):
                photo_size += 1

        return str(photo_size)
    else:
        return "Album not found", 404


# 输入用户名和相册名以及rank 返回按照时间排序后rank的图片
@app.route("/get_photos/<account>/<album_name>/<rank>", methods=["GET"])
def get_photos(account, album_name, rank):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
    if os.path.exists(album_path):
        photos = get_photo_list_in_timeorder(account, album_name)
        rank = int(rank.split(".")[0])

        if rank == -1:
            if len(photos) == 0:
                return send_file(default_path)
            photo_to_send = os.path.join(album_path, photos[0][0])
            return send_file(photo_to_send)

        if rank < len(photos):
            photo_to_send = os.path.join(album_path, photos[rank][0])
            print(photo_to_send)
            return send_file(photo_to_send)

        else:
            return send_file(default_path)
    else:
        return "Album not found", 404


# 输入用户名和相册名以及rank，删除按照时间排序后rank的图片
@app.route("/delete_photo/<account>/<album_name>/<rank>", methods=["GET"])
def delete_photo(account, album_name, rank):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)
    if os.path.exists(album_path):
        photos = get_photo_list_in_timeorder(account, album_name)
        rank = int(rank.split(".")[0])

        if rank < len(photos):
            photo_to_delete = os.path.join(album_path, photos[rank][0])
            print(photo_to_delete)
            os.remove(photo_to_delete)
            return "photo deleted"
        else:
            return "photo not found", 404
    else:
        return "Album not found", 404


@app.route("/get_moments", methods=["GET"])
def get_moments():
    data = {"moments": []}
    MomentManager()
    for i in range(len(MomentManager.json_data)-1, -1, -1):
        data["moments"].append(get_moments(i))
    return json.dumps(data)


@app.route("/upload_moments", methods=["POST"])
def upload_moments():
    user_name = request.form["account"]
    photo_size = int(request.form["size"])
    text = request.form["text"]
    print(user_name, photo_size, text)
    photos = get_photo_list_in_timeorder(user_name, "Moment")
    start_id = MomentManager.get_total_photosize_by_name(user_name)
    photos = photos[start_id : start_id + photo_size]
    MomentManager.add_moment(Moment(user_name, photos, text))
    MomentManager.save_json_data()
    return "moment upload"


def get_photo_list_in_timeorder(account, album_name):
    album_path = os.path.join(app.config["UPLOAD_FOLDER"], account, album_name)

    photos = {"photos": []}
    # 遍历相册目录下的所有文件
    for photo in os.listdir(album_path):
        # 如果是文件，将其加入photos字典中
        if os.path.isfile(os.path.join(album_path, photo)):
            photo_ctime = os.path.getctime(os.path.join(album_path, photo))
            photos["photos"].append((photo, photo_ctime))
    photos["photos"].sort(key=lambda x: x[1], reverse=False)

    return photos["photos"]


def get_moments(rank):
    moment: Moment = MomentManager.json_data[rank]
    info_to_send = {}
    info_to_send["UserName"] = moment.name
    info_to_send["Content"] = moment.text
    info_to_send["PhotoCount"] = len(moment.photo_list)
    info_to_send["PhotoUrls"] = []
    path = (moment.name, "Moment")

    if moment.name not in MomentManager.user_json_data:
        return info_to_send

    photos = get_photo_list_in_timeorder(path[0], path[1])
    for photo, ctime in moment.photo_list:
        try:
            i = photos.index((photo, ctime))
            info_to_send["PhotoUrls"].append(
                f"http://1.12.46.157/get_photos/{path[0]}/{path[1]}/{i}.jpg"
            )
        except ValueError:
            info_to_send["PhotoUrls"].append(
                f"http://1.12.46.157/get_photos/{path[0]}/{path[1]}/{-1}.jpg"
            )

    return info_to_send


if __name__ == "__main__":
    from waitress import serve

    serve(app, host="0.0.0.0", port=port)