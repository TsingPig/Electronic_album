from flask import Flask, request, send_file
from werkzeug.utils import secure_filename
import os

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

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=port)