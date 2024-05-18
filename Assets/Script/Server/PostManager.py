import pymysql
from typing import List, Dict

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
    def add_post(account: str, text: str, photo_list: List[str], section_id: int):
        if account is None:
            return False
        db, cursor = getconnection()
        cursor.execute("INSERT INTO postinfo (account, textinfo, sectionid) VALUES (%s, %s, %s)", (account, text, section_id))
        post_id = cursor.lastrowid
        for photo in photo_list:
            photo = f'{host}/get_photos/{account}/Post/{photo}'
            cursor.execute("INSERT INTO urlinfo (postid, url) VALUES (%s, %s)", (post_id, photo))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_posts_by_section(section_name: str) -> List[Dict]:
        data = []
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM sectioninfo WHERE sectionname = %s", (section_name))
        section = cursor.fetchone()
        cursor.execute("SELECT * FROM postinfo WHERE sectionid = %s", (section["sectionid"]))
        posts = cursor.fetchall()
        for post in reversed(posts):
            info_to_send = {}
            info_to_send["UserName"] = post["account"]
            info_to_send["Content"] = post["textinfo"]
            cursor.execute("SELECT * FROM urlinfo WHERE postid = %s", (post["postid"]))
            info_to_send["PhotoUrls"] = [photo["url"] for photo in cursor.fetchall()]
            info_to_send["PhotoCount"] = len(info_to_send["PhotoUrls"])
            data.append(info_to_send)
        cursor.close()
        db.close()
        return data
    

    

if __name__ == "__main__":
    pass