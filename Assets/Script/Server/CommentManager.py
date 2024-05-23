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
    def add_comment(account: str, post_id: int, text: str):
        db, cursor = getconnection()
        cursor.execute("INSERT INTO commentinfo (account, rootpostid, commentinfo) VALUES (%s, %s, %s)", (account, post_id, text))
        db.commit()
        cursor.close()
        db.close()
        return True