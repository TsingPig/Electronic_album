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


class SectionManager:
    @staticmethod
    def add_section(section_name: str):
        db, cursor = getconnection()
        cursor.execute("INSERT INTO sectioninfo (sectionname) VALUES (%s)", (section_name))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_sections() -> List[str]:
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM sectioninfo")
        sections = cursor.fetchall()
        cursor.close()
        db.close()
        return [section["sectionname"] for section in sections]
    
    @staticmethod
    def modify_section(old_name: str, new_name: str):
        db, cursor = getconnection()
        cursor.execute("UPDATE sectioninfo SET sectionname = %s WHERE sectionname = %s", (new_name, old_name))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def delete_section(section_name: str):
        db, cursor = getconnection()
        cursor.execute("DELETE FROM sectioninfo WHERE sectionname = %s", (section_name))
        db.commit()
        cursor.close()
        db.close()
        return True
    
    @staticmethod
    def get_sectionid_by_name(section_name: str) -> int:
        db, cursor = getconnection()
        cursor.execute("SELECT * FROM sectioninfo WHERE sectionname = %s", (section_name))
        section = cursor.fetchone()
        cursor.close()
        db.close()
        return section["sectionid"]