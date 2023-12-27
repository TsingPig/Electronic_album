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

        # λͼ��ʾҳ��������
        self.bitmap = [0] * num_pages



    def allocate_memory(self, num_pages):
        """
        :param num_pages:��Ҫ�����ҳ����
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