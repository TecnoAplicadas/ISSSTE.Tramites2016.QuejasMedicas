var PaginationHelper = (function () {
    function PaginationHelper(maxPages) {
        var _this = this;
        this.FIRST_PAGE = 1;
        this.getPages = function (selectedPage, totalPages) {
            var pages = [];
            var startPage = _this.FIRST_PAGE;
            var finalPage = totalPages;
            if (totalPages > _this.maxPages) {
                //Si tanto la primera como la última página estan alejadas más de la mitad de las paginas con respecto a la pagina seleccionada, ambas paginas se colocan a una distancia media
                if (selectedPage - _this.FIRST_PAGE > _this.maxPagesPerSide && totalPages - selectedPage > _this.maxPagesPerSide) {
                    startPage = selectedPage - _this.maxPagesPerSide;
                    finalPage = selectedPage + _this.maxPagesPerSide;
                }
                else if (selectedPage - _this.FIRST_PAGE > _this.maxPagesPerSide) {
                    startPage = selectedPage - (_this.maxPagesPerSide * 2) + (totalPages - selectedPage);
                }
                else if (totalPages - selectedPage > _this.maxPagesPerSide) {
                    finalPage = selectedPage + (_this.maxPagesPerSide * 2) - (selectedPage - _this.FIRST_PAGE);
                }
            }
            for (var i = startPage; i <= finalPage; i++)
                pages.push(i);
            return pages;
        };
        this.maxPages = maxPages;
        this.maxPagesPerSide = (this.maxPages - 1) / 2;
    }
    return PaginationHelper;
}());
//# sourceMappingURL=paginationHelper.js.map