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


class CommentManager:
    @staticmethod
    def add_comment(account: str, post_id: int, text: str) -> bool:
        db, cursor = getconnection()
        cursor.execute("INSERT INTO commentinfo (account, rootpostid, commentinfo) VALUES (%s, %s, %s)", (account, post_id, text))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_comments(post_id: int) -> List[Dict]:
        data = []
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM commentinfo WHERE rootpostid = %s", (post_id))
        comments = cursor.fetchall()
        for comment in comments:
            info_to_send = {}
            info_to_send["UserName"] = comment["account"]
            info_to_send["Content"] = comment["commentinfo"]
            info_to_send["CreateTime"] = str(comment["createtime"])
            info_to_send["CommentId"] = comment["commentid"]
            data.append(info_to_send)
        cursor.close()
        db.close()
        return data
    
    @staticmethod
    def delete_comment_by_id(comment_id: int) -> bool:
        db, cursor = getconnection()
        cursor.execute("DELETE FROM commentinfo WHERE commentid = %s", (comment_id))
        db.commit()
        cursor.close()
        db.close()
        return True