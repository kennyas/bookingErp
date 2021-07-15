"use strict";

//var customerDashboardConnection = new signalR
//    .HubConnectionBuilder()
//    .withUrl("http://localhost:7008/bookingsHub")
//    .configureLogging(signalR.LogLevel.Information)
//    .build();

//var salesDashboardConnection = new signalR
//    .HubConnectionBuilder()
//    .withUrl("http://localhost:7008/salesHub")
//    .configureLogging(signalR.LogLevel.Information)
//    .build();


var customerDashboardConnection = new signalR
    .HubConnectionBuilder()
    .withUrl("http://104.238.100.236:8010/bookingsHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

var salesDashboardConnection = new signalR
    .HubConnectionBuilder()
    .withUrl("http://104.238.100.236:8010/salesHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();


customerDashboardConnection.on("currentCustomerBookingsDashboardWithData", function (model) {
    console.log("Current customer bookings dashboard");
    console.log(model);
});

salesDashboardConnection.on("currentTotalSalesByTrip", function (model) {
    console.log("Current total sales by trip");
    console.log(model);
});

salesDashboardConnection.on("currentTotalSalesByVehicle", function (model) {
    console.log("Current total sales by vehicle");
    console.log(model);
});

salesDashboardConnection.on("currentTotalSales", function (model) {
    console.log("Current total sales");
    console.log(model);
});

salesDashboardConnection.on("currentSalesByTerminal", function (model) {
    console.log("Current sales by terminal");
    console.log(model);
});

salesDashboardConnection.on("currentSalesByRoute", function (model) {
    console.log("Current sales by route");
    console.log(model);
});

customerDashboardConnection.onclose(async () => {
    await startCustomerDashboardConnection();
});
salesDashboardConnection.onclose(async () => {
    await startSalesDashboardConnection();
});

async function startCustomerDashboardConnection() {
    try {
        await customerDashboardConnection.start();
        console.log("connected to customer bookings dashboard");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

async function startSalesDashboardConnection() {
    try {
        await salesDashboardConnection.start();
        console.log("connected to sales dashboard");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

// Start the connection.
startCustomerDashboardConnection();
startSalesDashboardConnection();