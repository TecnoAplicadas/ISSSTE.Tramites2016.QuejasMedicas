function paginationService() {

    this.maxPages = 11;
    this.paginationHelper = new PaginationHelper(this.maxPages);
    this.selectedPage = 1;
    this.totalPages = 1;
    this.pageSizes = [10, 20, 30, 40, 50];
    this.selectedPageSize = this.pageSizes[0];
    this.pages = [1];
    this.resultCount = 0;

    this.callBackFunction = function() {};


    this.isPageSelected = function(page) {
        return page == this.selectedPage;
    };
    this.isFirstPage = function() {
        return this.selectedPage == 1;
    };
    this.isLastPage = function() {
        return this.selectedPage == this.totalPages;
    };
    this.changeSelectedPage = function(page) {
        if (!this.isPageSelected(page)) {
            this.selectedPage = page;
            if (this.callBackFunction != null && this.callBackFunction != undefined) this.callBackFunction();
        }
    };
    this.changeToPreviousPage = function changeToPreviousPage() {
        if (!this.isFirstPage())
            this.changeSelectedPage(this.selectedPage - 1);
    };
    this.changeToNextPage = function() {
        if (!this.isLastPage())
            this.changeSelectedPage(this.selectedPage + 1);
    };
};