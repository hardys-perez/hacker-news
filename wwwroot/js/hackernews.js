class HackerNews {
    constructor() {
        var connection = new signalR.HubConnectionBuilder().withUrl("/tophackernews").build();

        connection.on("update", (message) => {
            this.drawTable(JSON.parse(message));
        });

        connection.start().then(function () {

        }).catch(function (err) {
            return console.error(err.toString());
        });

        this.$tableContent = $("#topNewsBodyId");
        this.$searchButton = $("#searchButtonId");
        this.$searchText = $("#searchTextId");
        this.$time = $("#timeId");
    }

    drawTable(rows) {
        this.$tableContent.html("");

        this.$time.html((new Date()).toLocaleTimeString());
        const filter = this.$searchText.val();

        for (var i = 0; i < rows.length; i++) {
            if (rows[i].By.includes(filter) || rows[i].Title.includes(filter)) {
                const htmlRow =
                    `<tr>
                    <td>${i + 1}</td>
                    <td>${rows[i].By}</td>
                    <td><a href="${rows[i].Url}" target="_blank">${rows[i].Title}</a></td>
                </tr>`;

                this.$tableContent.append(htmlRow);
            }
        }
    }
}

new HackerNews();