"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//Disable send button until connection is established


connection.on("ReceiveBooking", function (booking) {
    console.log(booking)

    const tabla = $("#table-booking");
    if (tabla) {
        tabla.append("<tr><td>" + booking.code + "</td><td>" + booking.name + "</td><td>" + formatDate(booking.checkIn) + "</td><td>" + formatDate(booking.checkOut) + "</td><td>" + booking.people + "</td><td>" + getStatus(booking.status) + "</td><td><a href='/BookingRegister/Edit/" + booking.id + "' class='btn btn-primary'>Editar</a></td></tr>");

    }

    $("#link-code").attr("href", "/BookingRegister/Edit/" + booking.id);
    $("#link-code").html(booking.code);

    $(".alert").removeClass("d-none");


    setTimeout(function () {
        $(".alert").addClass("d-none");
    }, 15000);

});

connection.start().then(function () {
    console.log("se conecto")
}).catch(function (err) {
    return console.error(err.toString());
});


function formatDate(date) {
    let d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [day, month, year].join('/');
}


function getStatus(status) {
    switch (status) {
        case 0:
            return "Pendiente";
        case 1:
            return "Confirmado";

        case 2:
            return "Cancelado";

    }
}