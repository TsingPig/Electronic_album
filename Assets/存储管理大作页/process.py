import time
from random import random

# ÿ�������������200���߼���ַ
num_random_logical_addresses = 200

class Process:
    def __init__(self, process_id, memory_manager, page_algorithm):
        self.process_id = process_id
        self.memory_manager = memory_manager
        self.page_algorithm = page_algorithm
        self.page_table = None
        self.page_faults = 0

    def run(self):
        print(f"Process {self.process_id} started.")
        for _ in range(num_random_logical_addresses):

            logical_address = self.generate_logical_address()
            self.access_memory(logical_address)

            sleep_time = self.generate_sleep_time()
            time.sleep(sleep_time)
        print(f"Process {self.process_id} finished. Page faults: {self.page_faults}")

    @staticmethod
    def generate_logical_address():
        """
        ÿ�����̵��߼���ַ�ռ�Ϊ2 ** 14�ֽ�
        :rtype: ����������ʵ��߼���ַ
        """
        return random.randint(0, 2 ** 14 - 1)

    @staticmethod
    def generate_sleep_time():
        """
        ÿ�ε�ַ���ʺ����ߣ�0-100ms)�е�һ�����ֵ
        :rtype: ÿ�����̵ķô�ʱ��Ϊ 0 - 100ms
        """
        return random.randint(0, 100) / 1000.0

    def access_memory(self, logical_address):
        # ���ʵ�ҳ�� = �߼���ַ // ҳ���С
        page_number = logical_address // self.memory_manager.page_size
        if self.page_table is None:
            self.page_table = self.memory_manager.allocate_memory(10)
        if page_number not in self.page_table:
            print(f"Process {self.process_id} Page fault at logical address {logical_address}")
            self.page_faults += 1
            if len(self.page_table) == 10:
                page_to_replace = self.page_algorithm.replace_page(self.memory_manager.memory, page_number,
                                                                   self.page_table[0])
                self.page_table = self.page_table[1:] + [page_number]
                self.memory_manager.deallocate_memory([page_to_replace])
            else:
                self.page_table.append(page_number)