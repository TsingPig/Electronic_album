from page_replacement_algorithm import FIFO, LRU
from memory_manager import MemoryManager, P, V
from process import Process

import threading

# �����ź�����ͬʱֻ����һ�����������ڴ�
mutex = threading.Semaphore(1)

# ��������
num_process = 12

# �������п���
running = True

memory_manager: MemoryManager = None

fifo_algorithm = lru_algorithm = None


def process_thread(i):
    while running:
        # �����ڴ�
        P(mutex)
        allocated_pages = memory_manager.allocate_memory(num_pages = 10)
        V(mutex)

        # ���벻��ʱ����
        if allocated_pages is None:
            continue


        # ���뵽�ڴ�󣬿�ʼִ�н���
        process = Process(i, memory_manager, fifo_algorithm)
        process.run()

        # �ͷ��ڴ�
        P(mutex)
        memory_manager.deallocate_memory(allocated_pages)
        V(mutex)


def main():
    global memory_manager, fifo_algorithm, lru_algorithm

    total_memory_size = 2 ** 14
    page_size = 256
    num_pages = total_memory_size // page_size
    memory_manager = MemoryManager(total_memory_size, page_size, num_pages)

    fifo_algorithm = FIFO()
    lru_algorithm = LRU()

    processes = [threading.Thread(target = process_thread, args = (i,)) for i in range(num_process)]
    for process in processes:
        process.start()

if __name__ == "__main__":
    main()
