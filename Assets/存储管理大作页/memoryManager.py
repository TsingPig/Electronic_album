import threading

def P(semaphore: threading.Semaphore):
    semaphore.acquire()

def V(semaphore: threading.Semaphore):
    semaphore.release()

class MemoryManager:
    def __init__(self, total_memory_size, page_size, num_pages):
        self.total_memory_size = total_memory_size
        self.page_size = page_size
        self.num_pages = num_pages
        self.free_pages = num_pages
        self.memory = [None] * total_memory_size

        self.processes = []

        # 位图显示页面分配情况
        self.bitmap = [0] * num_pages



    def allocate_memory(self, num_pages):
        """
        :param num_pages:需要分配的页面数
        """

        if self.free_pages < num_pages:
            return None



    def deallocate_memory(self, pages):
        """
        :param pages:
        """
        for page in pages:
            self.bitmap[page] = 0
            self.free_pages += 1