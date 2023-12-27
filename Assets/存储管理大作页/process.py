from memory_manager import MemoryManager
from page_replacement_algorithm import PageReplacementAlgorithm
import time
from random import random

# 每个进程随机生成200次逻辑地址
num_random_logical_addresses = 200


class Process:
    def __init__(self, process_id: int, memory_manager: MemoryManager, page_algorithm: PageReplacementAlgorithm):
        """
        初始化进程对象
        :param process_id: 进程id
        :param memory_manager: 内存管理器参数
        :param page_algorithm: 替换页面算法
        :param page_table: 页表, 个页表项占4个字节(系统的物理页面数为2 ** 6,每个页表正好占一个页面)
        :param cnt_page_faults: 缺页次数
        """
        self.process_id = process_id
        self.memory_manager: MemoryManager = memory_manager
        self.page_algorithm: PageReplacementAlgorithm = page_algorithm
        self.page_table = []
        self.cnt_page_faults = 0

    def run(self):
        print(f"进程 {self.process_id} 启动.")
        for _ in range(num_random_logical_addresses):
            logical_address = self.generate_logical_address()
            self.access_memory(logical_address)
            sleep_time = self.generate_sleep_time()
            time.sleep(sleep_time)
        print(f"进程 {self.process_id} 结束执行. Page faults: {self.cnt_page_faults}")

    @staticmethod
    def generate_logical_address():
        """
        每个进程的逻辑地址空间为2 ** 14字节
        :rtype: 返回随机访问的逻辑地址
        """
        return random.randint(0, 2 ** 14 - 1)

    @staticmethod
    def generate_sleep_time():
        """
        每次地址访问后休眠（0-100ms)中的一个随机值
        :rtype: 每个进程的访存时间为 0 - 100ms
        """
        return random.randint(0, 100) / 1000.0

    def access_memory(self, logical_address: int):
        """
        访问内存
        :param logical_address:逻辑地址
        """
        # 访问的页数 = 逻辑地址 // 页面大小
        page_number = logical_address // self.memory_manager.page_size

        self.memory_manager.cnt_access_memory += 1

        # if self.page_table is None:
        #     self.page_table = self.memory_manager.allocate_memory(10)

        if page_number not in self.page_table:
            print(f"Process {self.process_id} Page fault at logical address {logical_address}")
            # 缺页次数 + 1
            self.cnt_page_faults += 1
            self.memory_manager.cnt_page_fault += 1

            # 最多2 ** 6个页表项，满的时候需要替换
            if len(self.page_table) == 2 ** 6:
                # page_to_replace_idx 表示要替换的页表项的索引
                # page_to_replace 表示要替换的页表项
                page_replace_idx, page_to_replace = self.page_algorithm.replace_page(self.memory_manager.memory, page_number,
                                                                                     self.page_table)
                # 将要替换的页表项替换为新的页表项
                self.page_table[page_replace_idx] = page_number

                # 将要替换的页表项所在的物理页面释放
                self.memory_manager.deallocate_memory([page_to_replace])
            else:
                self.page_table.append(page_number)
