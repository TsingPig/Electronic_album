import pymysql
from typing import List, Dict
from PhotoManager import PhotoManager

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


class PostManager:
    @staticmethod
    def add_post(account: str, text: str, photo_list: List[str], title: str, section_id: int):
        if account is None:
            return False
        db, cursor = getconnection()
        cursor.execute("INSERT INTO postinfo (account, textinfo, title, sectionid) VALUES (%s, %s, %s, %s)", (account, text, title, section_id))
        post_id = cursor.lastrowid
        for photo in photo_list:
            photo = f'{host}/get_photos_byid/{account}/Post/{photo}'
            cursor.execute("INSERT INTO urlinfo (postid, url) VALUES (%s, %s)", (post_id, photo))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_posts_by_section(sectionname: str) -> List[Dict]:
        data = []
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM sectioninfo WHERE sectionname = %s", (sectionname))
        section = cursor.fetchone()
        cursor.execute("SELECT * FROM postinfo WHERE sectionid = %s", (section["sectionid"]))
        posts = cursor.fetchall()
        for post in reversed(posts):
            info_to_send = {}
            info_to_send["UserName"] = post["account"]
            info_to_send["Title"] = post["title"]
            info_to_send["Content"] = post["textinfo"]
            cursor.execute("SELECT * FROM urlinfo WHERE postid = %s", (post["postid"]))
            info_to_send["PhotoUrls"] = [photo["url"] for photo in cursor.fetchall()]
            info_to_send["PhotoCount"] = len(info_to_send["PhotoUrls"])
            info_to_send["CreateTime"] = str(post["createtime"])
            info_to_send["PostId"] = post["postid"]
            data.append(info_to_send)
        cursor.close()
        db.close()
        return data
    
    @staticmethod
    def delete_post_by_username_and_createtime(account: str, createtime: str) -> bool:
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM postinfo WHERE account = %s AND createtime = %s", (account, createtime))
        post = cursor.fetchone()
        if post is None:
            return False
        cursor.execute("DELETE FROM postinfo WHERE postid = %s", (post["postid"]))
        # TODO: 图床中的图片是否要删除
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def delete_post_by_id(post_id: int) -> bool:
        db, cursor = getconnection()
        # print(post_id)
        PhotoManager.delete_photos_by_postId(post_id)
        cursor.execute("DELETE FROM postinfo WHERE postid = %s", (post_id))
        db.commit()
        cursor.close()
        db.close()
        return True
    

    

if __name__ == "__main__":
    pass