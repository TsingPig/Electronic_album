class PageReplacementAlgorithm:
    def __init__(self):
        pass

    def replace_page(self, memory, page_to_load, page_table):
        idx, page_to_replace = 0, None  # 占位符
        return idx, page_to_replace


class FIFO(PageReplacementAlgorithm):
    def __init__(self):
        super().__init__()
        self.queue = []

    def replace_page(self, memory, page_to_load, page_table):
        idx, page_to_replace = 0, self.queue[0]  # 占位符
        return idx, page_to_replace


class LRU(PageReplacementAlgorithm):
    def __init__(self):
        super().__init__()
        self.order = []

    def replace_page(self, memory, page_to_load, page_table):
        idx, page_to_replace = 0, self.order[0]  # 占位符
        return idx, page_to_replace
