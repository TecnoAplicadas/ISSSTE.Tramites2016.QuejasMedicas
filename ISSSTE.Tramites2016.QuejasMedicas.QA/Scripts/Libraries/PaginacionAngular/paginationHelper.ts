class PaginationHelper {
    private FIRST_PAGE = 1;

    private maxPages: number;
    private maxPagesPerSide: number;

    constructor(maxPages: number) {
        this.maxPages = maxPages;
        this.maxPagesPerSide = (this.maxPages - 1) / 2;
    }

    getPages = (selectedPage: number, totalPages: number) => {
        var pages = [];
        var startPage = this.FIRST_PAGE;
        var finalPage = totalPages;

        if (totalPages > this.maxPages) {
            //Si tanto la primera como la última página estan alejadas más de la mitad de las paginas con respecto a la pagina seleccionada, ambas paginas se colocan a una distancia media
            if (selectedPage - this.FIRST_PAGE > this.maxPagesPerSide &&
                totalPages - selectedPage > this.maxPagesPerSide) {
                startPage = selectedPage - this.maxPagesPerSide;
                finalPage = selectedPage + this.maxPagesPerSide;
            }
            //Si solo la primera página esta alejádas más de la mitad de las páginas con respecto a la página seleccionada, la primera pagina se coloca lo más lejos que puede con respecto al máximo menos las paginas siguientes
            else if (selectedPage - this.FIRST_PAGE > this.maxPagesPerSide) {
                startPage = selectedPage - (this.maxPagesPerSide * 2) + (totalPages - selectedPage);
            }
            //Si solo la última página esta alejádas más de la mitad de las páginas con respecto a la página seleccionada, la última pagina se coloca lo más lejos que puede con respecto al máximo menos las paginas anteriores
            else if (totalPages - selectedPage > this.maxPagesPerSide) {
                finalPage = selectedPage + (this.maxPagesPerSide * 2) - (selectedPage - this.FIRST_PAGE);
            }
        }

        for (let i = startPage; i <= finalPage; i++)
            pages.push(i);

        return pages;
    };
}