$(function () {

    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json(); //this returns a promise, so we await it
                buildEmployeeList(payload);
                msg === "" ? $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`); //are we appending to an existing message
            } else if (response.status !== 404) { //probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {//else 404 not found
                $("#status").text("no such path on server");
            }

            response = await fetch(`api/department`);
            if (response.ok) {
                let deps = await response.json(); //this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(deps));
            } else if (response.staus !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("no such path on server");
            } //else
        } catch (error) {
            $("#status").text(error.message);
        }
    }; //getAll

    const setupForUpdate = (id, data) => {
        $("#deletebutton").show();
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");

        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstname").val(employee.firstname);
                $("#TextBoxLastname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);
                sessionStorage.setItem("id", employee.id);
                sessionStorage.setItem("departmentId", employee.departmentId);
                sessionStorage.setItem("timer", employee.timer);
                $("#modalstatus").text("update data");
                loadDepartmentDDL(employee.departmentId);
                $("#myModal").modal("toggle");
                $("#myModalLabel").text("Update");
            } //if
        }); //data.map
    }; //setupForUpdate

    const setupForAdd = () => {
        $("#deletebutton").hide();
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#myModal").modal("toggle");
        $("#modalstatus").text("add new employee");
        $("#myModalLabel").text("Add");
        clearModalFields();
    }; //setupForAdd


    const clearModalFields = () => {
        loadDepartmentDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId");
        sessionStorage.removeItem("timer");
    }; //clearModalFields

    const loadDepartmentDDL = (empdep) => {
        html = '';
        $('#ddlDepartments').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.map(dep => html += `<option value="${dep.id}">${dep.name}</option>`);
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(empdep);
    }; //loadDivisionDDL

    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            emp.departmentId = parseInt($("#ddlDepartments").val()); //hard code it for now, we'll add a dropdown later
            emp.id = -1;
            emp.timer = null;
            emp.picture64 = null;
            //send the employee info to the server asynchronously using POST
            let response = await fetch("api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) //or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            }
            else {
                $("#status").text("no such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
        $("#myModal").modal("toggle");
    }; //add


    const _delete = async () => {
        try {
            let response = await fetch(`api/employee/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - $(response.status}, Problem on delete server side, see server console`);
            }
            $('#myModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; //delete

    const update = async () => {
        try {
            //set up a new client side instance of Employee
            emp = new Object();
            //populate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            //we stored these 3 earlier
            emp.id = parseInt(sessionStorage.getItem("id"));
            emp.departmentId = parseInt($("#ddlDepartments").val());
            emp.timer = sessionStorage.getItem("timer");
            emp.picture64 = null;

            //send the updated back to the server asynchronously using PUT
            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp)
            });

            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("no such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
        $("#myModal").modal("toggle");

    }//update

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    $("#employeeList").click((e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        } //clicked on row somewhere else

        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; //ignore if they clicked on heading or status
        }
    }); //employeeList lick

    $("#deletebutton").click(() => {
        if (window.confirm("Are you sure"))
            _delete();
    });

    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-info row d-flex" id="status">Employee Info</div>
                    <div class= "list-group-item row d-flex text-center" id="heading">
                    <div class="col-4 h4">Title</div>
                    <div class="col-4 h4">First</div>
                    <div class="col-4 h4">Last</div>
                </div>`);
        div.appendTo($("#employeeList"));
        sessionStorage.setItem("allemployees", JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`);
        btn.appendTo($("#employeeList"));
        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
                      <div class="col-4" id="employeefirstname${emp.id}">${emp.firstname}</div>
                      <div class="col-4" id="employeelastname${emp.id}">${emp.lastname}</div>`
            );
            btn.appendTo($("#employeeList"));
        }); //map
    }; //buildEmployeeList

    getAll(""); //first grab the data from the server
}); //jQuery ready method

//server was reached but had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors);
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0] //first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    }// else
}