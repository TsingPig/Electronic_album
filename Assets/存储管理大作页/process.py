from memory_manager import MemoryManager
from page_replacement_algorithm import PageReplacementAlgorithm
import time
from random import random

# ÿ�������������200���߼���ַ
num_random_logical_addresses = 200


class Process:
    def __init__(self, process_id: int, memory_manager: MemoryManager, page_algorithm: PageReplacementAlgorithm):
        """
        ��ʼ�����̶���
        :param process_id: ����id
        :param memory_manager: �ڴ����������
        :param page_algorithm: �滻ҳ���㷨
        :param page_table: ҳ��, ��ҳ����ռ4���ֽ�(ϵͳ������ҳ����Ϊ2 ** 6,ÿ��ҳ������ռһ��ҳ��)
        :param cnt_page_faults: ȱҳ����
        """
        self.process_id = process_id
        self.memory_manager: MemoryManager = memory_manager
        self.page_algorithm: PageReplacementAlgorithm = page_algorithm
        self.page_table = []
        self.cnt_page_faults = 0

    def run(self):
        print(f"���� {self.process_id} ����.")
        for _ in range(num_random_logical_addresses):
            logical_address = self.generate_logical_address()
            self.access_memory(logical_address)
            sleep_time = self.generate_sleep_time()
            time.sleep(sleep_time)
        print(f"���� {self.process_id} ����ִ��. Page faults: {self.cnt_page_faults}")

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

    def access_memory(self, logical_address: int):
        """
        �����ڴ�
        :param logical_address:�߼���ַ
        """
        # ���ʵ�ҳ�� = �߼���ַ // ҳ���С
        page_number = logical_address // self.memory_manager.page_size

        self.memory_manager.cnt_access_memory += 1

        # if self.page_table is None:
        #     self.page_table = self.memory_manager.allocate_memory(10)

        if page_number not in self.page_table:
            print(f"Process {self.process_id} Page fault at logical address {logical_address}")
            # ȱҳ���� + 1
            self.cnt_page_faults += 1
            self.memory_manager.cnt_page_fault += 1

            # ���2 ** 6��ҳ�������ʱ����Ҫ�滻
            if len(self.page_table) == 2 ** 6:
                # page_to_replace_idx ��ʾҪ�滻��ҳ���������
                # page_to_replace ��ʾҪ�滻��ҳ����
                page_replace_idx, page_to_replace = self.page_algorithm.replace_page(self.memory_manager.memory, page_number,
                                                                                     self.page_table)
                # ��Ҫ�滻��ҳ�����滻Ϊ�µ�ҳ����
                self.page_table[page_replace_idx] = page_number

                # ��Ҫ�滻��ҳ�������ڵ�����ҳ���ͷ�
                self.memory_manager.deallocate_memory([page_to_replace])
            else:
                self.page_table.append(page_number)
