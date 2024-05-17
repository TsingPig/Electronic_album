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


class MomentManager:
    @staticmethod
    def add_moment(account: str, text: str, photo_list: List[str]):
        if account is None:
            return False
        db, cursor = getconnection()
        cursor.execute("INSERT INTO postinfo (account, textinfo) VALUES (%s, %s)", (account, text))
        moment_id = cursor.lastrowid
        for photo in photo_list:
            cursor.execute("INSERT INTO urlinfo (postid, url) VALUES (%s, %s)", (moment_id, photo))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_moments():
        data = {"moments": []}
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM postinfo")
        moments = cursor.fetchall()
        for moment in moments:
            info_to_send = {}
            info_to_send["UserName"] = moment["account"]
            info_to_send["Content"] = moment["textinfo"]
            cursor.execute("SELECT * FROM urlinfo WHERE postid = %s", (moment["postid"]))
            path = (moment["account"], "Moment")
            info_to_send["PhotoUrls"] = [f'{host}/get_photos_byid/{path[0]}/{path[1]}/{photo["url"]}' for photo in cursor.fetchall()]
            info_to_send["PhotoCount"] = len(info_to_send["PhotoUrls"])
            data["moments"].append(info_to_send)
        cursor.close()
        db.close()
        return data

    

if __name__ == "__main__":
    # MomentManager.add_moment("test2", "test2", ["test2"])
    print(MomentManager.get_moments())