$(function () {
    $('#downloadFile').click(function () {
        $('#downloadFileInput').click();
    });

    $('#downloadFileInput').change(function (e) {
        var files = e.target.files;
        if (files.length > 0) {
            if (this.value.lastIndexOf('.csv') === -1) {
                alert('Only csv files are allowed!');
                this.value = '';
                return;
            }
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }

                $.ajax({
                    type: "POST",
                    url: '/api/MatrixApi/GetMatrixFromFile',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (data) {
                        drawMatrix(data);
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }
                });
            } else {
                alert("This browser doesn't support HTML5 file uploads!");
            }
        }
    });

    $('#uploadMatrix').click(function () {
        if (!$("#matrix").has("table").length) return;
        var data = getMatrixFromTable();
        if (data.length == 0 || data[0].length == 0) return;
        window.location.href = "/api/MatrixApi/ExportMatrixToFile?" + createMatrixQueryString(data);
    });

    $('#randomMatrix').click(function () {
        $.ajax({
            url: "/api/MatrixApi/GetRandomMatrix",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                drawMatrix(data);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });
    });

    $('#rotateMatrix').click(function () {
        if (!$("#matrix").has("table").length) return;
        var data = getMatrixFromTable();
        if (data.length == 0 || data[0].length == 0) return;
        $.ajax({
            url: "/api/MatrixApi/RotateMatrix",
            contentType: "application/json; charset=utf-8",
            data: {
                matrix: data
            },
            dataType: "json",
            success: function (data) {
                drawMatrix(data);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });
    });

    function drawMatrix(matrix) {
        var div = document.getElementById('matrix');
        div.innerHTML = "";
        var tbl = document.createElement('table');
        var tbdy = document.createElement('tbody');
        for (var i = 0; i < matrix.length; i++) {
            var tr = document.createElement('tr');
            for (var j = 0; j < matrix[i].length; j++) {
                var td = document.createElement('td');
                td.appendChild(document.createTextNode(matrix[i][j]));
                tr.appendChild(td);
            }
            tbdy.appendChild(tr);
        }
        tbl.appendChild(tbdy);
        div.appendChild(tbl);
    }
    function getMatrixFromTable() {
        var matrix = [];
        $("#matrix tr").each(function () {
            var line = [];
            var data = $(this).find('td');
            data.each(function () { line.push($(this).text()); });
            matrix.push(line);
        });
        return matrix;
    }
    function createMatrixQueryString(matrix) {
        var qs = '';
        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < matrix[i].length; j++) {
                qs += 'matrix%5B' + i + '%5D%5B%5D=' + matrix[i][j] + "&";
            }
        }
        return qs.slice(0, -1);
    }
});