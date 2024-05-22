$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();

    connection.on("LoadAccount", function () {
        alert("Accounts loaded successfully!");
        location.href = '/Admin/Accounts';
    })
    connection.on("LoadProducts", function () {
        alert("Products loaded successfully!");
        location.href = '/Admin/Products';
    });
    connection.on("LoadOrdersOnline", function () {
        alert("Orders Online loaded successfully!");
        location.href = '/OnlineProducts';
    });
    connection.on("LoadCategories", function () {
        location.href = '/Admin/Categories';
    });
})