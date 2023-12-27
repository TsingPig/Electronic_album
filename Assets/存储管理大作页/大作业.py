import random
import time


class MemoryManager:
    def __init__(self, total_memory_size, page_size, num_pages):
        self.total_memory_size = total_memory_size
        self.page_size = page_size
        self.num_pages = num_pages
        self.bitmap = [0] * num_pages
        self.free_pages = num_pages
        self.memory = [None] * total_memory_size
        self.processes = []

    def allocate_memory(self, num_pages):
        if self.free_pages < num_pages:
            return None
        allocated_pages = []
        for i in range(self.num_pages):
            if self.bitmap[i] == 0:
                self.bitmap[i] = 1
                allocated_pages.append(i)
                self.free_pages -= 1
                if len(allocated_pages) == num_pages:
                    break
        return allocated_pages

    def deallocate_memory(self, pages):
        for page in pages:
            self.bitmap[page] = 0
            self.free_pages += 1


class PageReplacementAlgorithm:
    def __init__(self):
        pass

    def replace_page(self, memory, page_to_load, page_to_replace):
        pass


class FIFO(PageReplacementAlgorithm):
    def __init__(self):
        super().__init__()
        self.queue = []

    def replace_page(self, memory, page_to_load, page_to_replace):
        # FIFO algorithm: Replace the oldest page in the queue
        oldest_page = self.queue.pop(0)
        memory[oldest_page * memory.page_size: (oldest_page + 1) * memory.page_size] = \
            memory[page_to_load * memory.page_size: (page_to_load + 1) * memory.page_size]
        self.queue.append(page_to_load)
        return oldest_page


class LRU(PageReplacementAlgorithm):
    def __init__(self):
        super().__init__()
        self.order = []

    def replace_page(self, memory, page_to_load, page_to_replace):
        # LRU algorithm:
        if self.order:
            least_recently_used = self.order.pop(0)
            memory[least_recently_used * memory.page_size: (least_recently_used + 1) * memory.page_size] = \
                memory[page_to_load * memory.page_size: (page_to_load + 1) * memory.page_size]
            self.order.append(page_to_load)
            return least_recently_used
        else:
            return None


class Process:
    def __init__(self, process_id, memory_manager, page_algorithm):
        self.process_id = process_id
        self.memory_manager = memory_manager
        self.page_algorithm = page_algorithm
        self.page_table = None
        self.page_faults = 0

    def run(self):
        print(f"Process {self.process_id} started.")
        for _ in range(200):
            logical_address = self.generate_logical_address()
            self.access_memory(logical_address)
            sleep_time = random.randint(0, 100) / 1000.0
            time.sleep(sleep_time)
        print(f"Process {self.process_id} finished. Page faults: {self.page_faults}")

    def generate_logical_address(self):
        # Simplified logic for generating logical addresses
        return random.randint(0, 8191)

    def access_memory(self, logical_address):
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


def main():
    total_memory_size = 2 ** 14
    page_size = 256
    num_pages = total_memory_size // page_size
    memory_manager = MemoryManager(total_memory_size, page_size, num_pages)

    fifo_algorithm = FIFO()
    lru_algorithm = LRU()

    processes = []
    for i in range(12):
        process = Process(i, memory_manager, lru_algorithm if i % 2 == 0 else fifo_algorithm)
        processes.append(process)

    for process in processes:
        process.run()


if __name__ == "__main__":
    main()
