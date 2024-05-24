# 用于管理图床中图片，主要作用是当数据库信息发生变化时，更新图床中的图片信息
import pymysql
from typing import List, Dict
import os

host = "http://114.132.233.105"
def getconnection():
    # 连接数据库
    db = pymysql.connect(host='cd-cdb-8py1pna8.sql.tencentcdb.com', 
                        port=23211,
                        user='root', 
                        password='114514_191980', 
                        database='database', 
                        charset='utf8')
    return db, db.cursor(cursor=pymysql.cursors.DictCursor)


class PhotoManager:
    @staticmethod
    def delete_photo_by_path(path: str):
        if os.path.exists(path):
            os.remove(path)

    @staticmethod
    def delete_photos_by_postId(postid: int):
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM urlinfo WHERE postid = %s", (postid))
        photos = cursor.fetchall()
        for photo in photos:
            url = photo["url"]
            path = url.split("/")[-1:-4:-1]
            photo_to_delete = os.path.join("uploads", path[2], path[1], path[0])
            # print(photo_to_delete)
            PhotoManager.delete_photo_by_path(photo_to_delete)