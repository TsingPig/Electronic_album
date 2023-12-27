from flask import Flask, request, send_file
from werkzeug.utils import secure_filename
import os
import json

app = Flask(__name__)
app.config['UPLOAD_FOLDER'] = 'uploads'
app.config['MAX_CONTENT_LENGTH'] = 16 * 1024 * 1024  # 设置最大上传文件大小为16MB
port = 80

# 处理文件上传
@app.route('/upload', methods=['POST'])
def upload_file():
    if 'file' not in request.files:
        return 'No file part', 400
    file = request.files['file']
    if file.filename == '':
        return 'No selected file', 400
    if file:
        filename = secure_filename(file.filename)
        account = request.form['account']
        upload_path = os.path.join(app.config['UPLOAD_FOLDER'], account)
        if not os.path.exists(upload_path):
            os.makedirs(upload_path)
        file.save(os.path.join(upload_path, filename))
        return 'File uploaded!'

# 创建相册
@app.route('/createEmptyFolder/<account>/<album_name>', methods=['POST'])
def create_album(account, album_name):
    album_path = os.path.join(app.config['UPLOAD_FOLDER'], account, album_name)
    if not os.path.exists(album_path):
        os.makedirs(album_path)
        return 'Album created successfully!'
    else:
        return 'Album already exists.'

# 处理文件下载
@app.route('/download/<account>/<filename>', methods=['GET'])
def download_file(account, filename):
    file_path = os.path.join(app.config['UPLOAD_FOLDER'], account, filename)
    if os.path.exists(file_path):
        return send_file(file_path)
    else:
        return 'File not found', 404
    
# 输入用户名，返回对应用户名下的相册列表
@app.route('/get_folders/<account>', methods=['GET'])
def get_folders(account):
    album_path = os.path.join(app.config['UPLOAD_FOLDER'], account)
    folders = {'folders': []}
    if os.path.exists(album_path):
        # 遍历用户目录下的所有文件夹
        for folder in os.listdir(album_path):
            # 如果是文件夹，将其加入folders字典中
            if os.path.isdir(os.path.join(album_path, folder)):
                folders['folders'].append(folder)
        print(folders['folders'])
        return json.dumps(folders)
    else:
        return json.dumps(folders)

# 输入用户名和相册名，返回对应相册下的图片数量
@app.route('/connect_size/<account>/<album_name>', methods=['GET'])
def connect_size(account, album_name):
    album_path = os.path.join(app.config['UPLOAD_FOLDER'], account, album_name)
    if os.path.exists(album_path):
        photo_size = 0
        for photo in os.listdir(album_path):
            if os.path.isfile(os.path.join(album_path, photo)):
                photo_size += 1
    
        print(photo_size)
        return photo_size
    else:
        return 'Album not found', 404

# 输入用户名和相册名以及range: l-r 返回按照时间排序后[l, r]的图片
@app.route('/get_photos/<account>/<album_name>/<range>', methods=['GET'])
def get_photos(account, album_name, range):
    album_path = os.path.join(app.config['UPLOAD_FOLDER'], account, album_name)
    if os.path.exists(album_path):
        photos = {'photos': []}
        # 遍历相册目录下的所有文件
        for photo in os.listdir(album_path):
            # 如果是文件，将其加入photos字典中
            if os.path.isfile(os.path.join(album_path, photo)):
                photo_ctime = os.path.getctime(os.path.join(album_path, photo))
                photos['photos'].append((photo, photo_ctime))           
        photos['photos'].sort(key=lambda x: x[1], reverse=True)
        # 如果range为all，返回所有图片
        if range == 'all':
            print(photos['photos'])
            a = {}
            for item in photos.items():
                a[item.key] = item.value[1]
            return json.dumps(a)
            for photo, _ in photos['photos']:
                send_file(photo)
        # 否则返回range范围内的图片
        else:
            l = int(range.split('-')[0])
            r = int(range.split('-')[1])
            return json.dumps(photos['photos'][l:r+1])
            for photo, _ in photos['photos'][l:r+1]:
                send_file(photo)
    else:
        return 'Album not found', 404

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=port)