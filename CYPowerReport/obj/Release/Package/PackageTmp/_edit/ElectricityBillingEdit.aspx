<%@ Page Title="" Language="C#" MasterPageFile="~/_master/Edit.Master" AutoEventWireup="true" CodeBehind="ElectricityBillingEdit.aspx.cs" Inherits="WebApp._edit.ElectricityBillingEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .autocomplete {
            position: relative;
            display: inline-block;
        }



        .autocomplete-items {
            position: absolute;
            border: 1px solid #d4d4d4;
            border-bottom: none;
            border-top: none;
            z-index: 99;
            /*position the autocomplete items to be the same width as the container:*/
            top: 100%;
            left: 0;
            right: 0;
        }

            .autocomplete-items div {
                padding: 10px;
                cursor: pointer;
                background-color: #fff;
                border-bottom: 1px solid #d4d4d4;
            }

                /*when hovering an item:*/
                .autocomplete-items div:hover {
                    background-color: #e9e9e9;
                }

        /*when navigating through the items using the arrow keys:*/
        .autocomplete-active {
            background-color: DodgerBlue !important;
            color: #ffffff;
        }
    </style>
    <script type="text/javascript">
        function checkData() {
            var msg = "";

            if (msg.length > 0) {
                alert(msg);
                return false;
            }

        }

        function autocomplete(inp, arr) {
            /*the autocomplete function takes two arguments,
            the text field element and an array of possible autocompleted values:*/
            var currentFocus;
            /*execute a function when someone writes in the text field:*/
            inp.addEventListener("input", function (e) {
                var a, b, i, val = this.value;
                /*close any already open lists of autocompleted values*/
                closeAllLists();
                //if (!val) { return false; }
                currentFocus = -1;
                /*create a DIV element that will contain the items (values):*/
                a = document.createElement("DIV");
                a.setAttribute("id", this.id + "autocomplete-list");
                a.setAttribute("class", "autocomplete-items");
                /*append the DIV element as a child of the autocomplete container:*/
                this.parentNode.appendChild(a);
                /*for each item in the array...*/
                for (i = 0; i < arr.length; i++) {
                    /*check if the item starts with the same letters as the text field value:*/
                    //if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                    if (val == "" || arr[i].includes(val.toUpperCase())) {
                        /*create a DIV element for each matching element:*/
                        b = document.createElement("DIV");
                        /*make the matching letters bold:*/
                        b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                        b.innerHTML += arr[i].substr(val.length);
                        /*insert a input field that will hold the current array item's value:*/
                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                        /*execute a function when someone clicks on the item value (DIV element):*/
                        b.addEventListener("click", function (e) {
                            /*insert the value for the autocomplete text field:*/
                            inp.value = this.getElementsByTagName("input")[0].value;
                            setValue();
                            /*close the list of autocompleted values,
                            (or any other open lists of autocompleted values:*/
                            closeAllLists();
                        });
                        a.appendChild(b);
                    }
                }

            });
            /*execute a function presses a key on the keyboard:*/
            inp.addEventListener("keydown", function (e) {
                var x = document.getElementById(this.id + "autocomplete-list");
                if (x) x = x.getElementsByTagName("div");
                if (e.keyCode == 40) {
                    /*If the arrow DOWN key is pressed,
                    increase the currentFocus variable:*/
                    currentFocus++;
                    /*and and make the current item more visible:*/
                    addActive(x);
                } else if (e.keyCode == 38) { //up
                    /*If the arrow UP key is pressed,
                    decrease the currentFocus variable:*/
                    currentFocus--;
                    /*and and make the current item more visible:*/
                    addActive(x);
                } else if (e.keyCode == 13) {
                    /*If the ENTER key is pressed, prevent the form from being submitted,*/
                    e.preventDefault();
                    if (currentFocus > -1) {
                        /*and simulate a click on the "active" item:*/
                        if (x) x[currentFocus].click();
                    }
                }
            });
            function addActive(x) {
                /*a function to classify an item as "active":*/
                if (!x) return false;
                /*start by removing the "active" class on all items:*/
                removeActive(x);
                if (currentFocus >= x.length) currentFocus = 0;
                if (currentFocus < 0) currentFocus = (x.length - 1);
                /*add class "autocomplete-active":*/
                x[currentFocus].classList.add("autocomplete-active");
            }
            function removeActive(x) {
                /*a function to remove the "active" class from all autocomplete items:*/
                for (var i = 0; i < x.length; i++) {
                    x[i].classList.remove("autocomplete-active");
                }
            }
            function closeAllLists(elmnt) {
                /*close all autocomplete lists in the document,
                except the one passed as an argument:*/
                var x = document.getElementsByClassName("autocomplete-items");
                for (var i = 0; i < x.length; i++) {
                    if (elmnt != x[i] && elmnt != inp) {
                        x[i].parentNode.removeChild(x[i]);
                    }
                }
            }
            /*execute a function when someone clicks in the document:*/
            document.addEventListener("click", function (e) {
                closeAllLists(e.target);
            });
        }

        /*An array containing all the country names in the world:*/
        var countries = ["Afghanistan", "Albania", "測試", "Andorra", "Angola", "Anguilla", "Antigua & Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia & Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central Arfrican Republic", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauro", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palau", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre & Miquelon", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "St Kitts & Nevis", "St Lucia", "St Vincent", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks & Caicos", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];

        setValue = () => {
            console.log(3)
            setTimeout(() => {

                var textToFind = $("#myInput").val();
                var dd = $("#TabContent_ddlCBD_Case_Name")[0];
                for (var i = 0; i < dd.options.length; i++) {
                    if (dd.options[i].text === textToFind) {
                        dd.selectedIndex = i;
                        break;
                    }
                }
                //__doPostBack('ctl00$TabContent$ddlCBD_Case_Name', '')
                javascript: setTimeout('__doPostBack(\'ctl00$TabContent$ddlCBD_Case_Name\',\'\')', 0)
            }, 500)


        }

        setCaseNames = () => {
            console.log(2)
            var a = $("#TabContent_ddlCBD_Case_Name")[0].options;
            countries = [...a].map(x => x.text);
            autocomplete($("#myInput")[0], countries);
        }
        /*initiate the autocomplete function on the "myInput" element, and pass along the countries array as possible autocomplete values:*/

        hideIfEdit = () => {
            if ($("#TabContent_ddlCBD_Case_Name").is('[disabled=disabled]')) {
                setTimeout(() => { $("#searchBar").hide(); }, 500)

            }
        }
        hideIfEdit();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
    <%--<asp:HiddenField ID="hidID" runat="server" />--%>
    <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClientClick="return checkData()" />
    <input id="btnCancel" type="button" value="取消" onclick="parent.CloseAndReload(1, 0);" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabTitle" runat="server">
    <li class='tab'><a href="#tabs-1">電費報表</a></li>
</asp:Content>
<asp:Content ID="Content4" UseSubmitBehavior="false" ContentPlaceHolderID="TabContent" runat="server">
    <div id="tabs-1" style="padding-top: 3px;" autocomplete="off">
        <table style="width: 100%;">
            <tr>
                <th class="label must">案場名稱</th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCD_TYPE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCD_TYPE_SelectedIndexChanged"></asp:DropDownList>
                    <asp:DropDownList ID="ddlCBD_Case_Name" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCBD_Case_Name_SelectedIndexChanged"></asp:DropDownList>
                    <div id="searchBar" runat="server" style="display: block; width: 300px;" class="autocomplete" >
                        搜尋：
                        <input id="myInput" autocomplete="off" type="text" onfocus="setCaseNames()">
                    </div>
                </td>
            </tr>
            <tr>
                <th class="label write">上次抄表日期</th>
                <td>
                    <uc:ucDate ID="dteEC_Last_Meter_Reading" runat="server" autocomplete="off" />
                    <p></p>
                    計費時間(起)=上次抄表日期
                </td>
                <th class="label write">本次抄表日期</th>
                <td>
                    <uc:ucDate ID="dteEC_Current_Meter_Reading" runat="server" onchange="getEC_Day();" autocomplete="off" />
                    <p></p>
                    計費時間(止)=本次抄表時間前一日
                </td>
            </tr>
            <tr>
                <th class="label write">下次抄表日期</th>
                <td>
                    <uc:ucDate ID="dteEC_Next_Meter_Reading" runat="server" autocomplete="off" />
                </td>
                <th class="label write">天數</th>
                <td>
                    <asp:TextBox ID="txtEC_Days" runat="server" Width="95%" MaxLength="3" class="CheckNum" onchange="EC_Day();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">抄表紀錄 公司購電-度數</th>
                <td>
                    <asp:TextBox ID="txtEC_Meter_Record" runat="server" Width="95%" MaxLength="10" class="CheckDecimal_2" onchange="Meter_Record();"></asp:TextBox>
                </td>
                <th class="label write">電費計算費率</th>
                <td>
                    <asp:TextBox ID="txtEC_Calculation_Rate" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_2" onchange="Calculation_Rate();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">電費計算 計費度數</th>
                <td>
                    <asp:TextBox ID="txtEC_Calculation_Record" runat="server" Width="95%" MaxLength="10" class="CheckDecimal_2" onchange="Calculation_Record();"></asp:TextBox>
                </td>
                <th class="label write">電費計算金額</th>
                <td>
                    <asp:TextBox ID="txtEC_Calculation_Amt" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_2" onchange="Calculation_Amt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">日金額</th>
                <td>
                    <asp:TextBox ID="txtEC_Daily_Amount" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">日購電度數</th>
                <td>
                    <asp:TextBox ID="txtEC_Duarantee_Rate" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">日計費度數</th>
                <td>
                    <asp:TextBox ID="txtEC_Daily_Billing" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">驗算</th>
                <td>
                    <asp:TextBox ID="txtEC_Check_Amount" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
            </tr>
            <%--「電表租費」、「手續費(匯費)」、「發票日期」、「發票號碼」、「發票金額」、「應付總金額」、「實際入帳金額」--%>
            <div style="display: none">
                <asp:TextBox Visible="true" ID="txtCBDTotalChangeRate" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                taxrate
                <asp:TextBox Visible="true" ID="txtCBDTaxRate" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                kw
                <asp:TextBox Visible="true" ID="txtKW" runat="server" Width="95%" MaxLength="19"></asp:TextBox>

                <%--<asp:TextBox Visible="true" ID="txtCBDRemittanceFee" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1"></asp:TextBox>--%>
                <%--<asp:TextBox Visible="true" ID="txtCBDMeterRent" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1"></asp:TextBox>--%>
            </div>
            <tr>
                <th class="label write">本期電表租費</th>
                <td>
                    <asp:TextBox ID="txtMeterRent" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">手續費(匯費)</th>
                <td>
                    <asp:TextBox ID="txtRemittanceFee" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">發票日期</th>
                <td>
                    <uc:ucDate ID="txtInvoiceDate" runat="server" />
                </td>
                <th class="label write">發票號碼</th>
                <td>
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="95%" MaxLength="19" onchange="autoFill()"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">發票金額</th>
                <td>
                    <asp:TextBox ID="txtInvoicePrice" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">應付總金額</th>
                <td>
                    <asp:TextBox ID="txtTotalPrice" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">營業稅</th>
                <td>
                    <asp:TextBox ID="txtBusinessTax" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">實際入帳金額</th>
                <td>
                    <asp:TextBox ID="txtActualIncome" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_1" onchange="CheckNumber();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">線路損失率</th>
                <td>
                    <asp:TextBox ID="txtEC_Line_Loss" runat="server" Width="95%" MaxLength="19" class="CheckDecimal_2" onchange="CheckNumber();"></asp:TextBox>
                </td>
                <th class="label write">銀行帳戶</th>
                <td>
                    <asp:TextBox ID="txtBankAccount" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">地主</th>
                <td>
                    <asp:TextBox ID="txtLandOwner" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
                <th class="label write">銀行別</th>
                <td>
                    <asp:TextBox ID="txtBankType" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">掛號寄出日期</th>
                <td>
                    <uc:ucDate ID="txtMailDate" runat="server" />
                </td>
                <th class="label write">台電部門</th>
                <td>
                    <asp:TextBox ID="txtECDepartment" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">台電計費月份</th>
                <td>
                    <asp:TextBox ID="txtECMonth" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
                <th class="label write">日平均</th>
                <td>
                    <asp:TextBox ID="txtDayAvg" title="購電度數/天數/案場KW" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">公司名稱</th>
                <td>
                    <asp:TextBox ID="txtCompanyName" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
                <th class="label write">設置容量</th>
                <td>
                    <asp:TextBox ID="txtSetAmount" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="label write">地址</th>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
                <th class="label write">備註</th>
                <td>
                    <asp:TextBox ID="txtComment" runat="server" Width="95%" MaxLength="19"></asp:TextBox>
                </td>
            </tr>






        </table>
    </div>
    <script type="text/javascript">
        autoFill = () => {
            getEC_Day();
            EC_Day();
            Calculation_Amt();
            Meter_Record();
            CalDaily_Amount();
            CalDuarantee_Rate();
            CalDaily_Billing();
            Check_Amount();
            roundAllDecimal();
            CheckNumber();
        }


        //計算天數
        function getEC_Day() {
            //alert("getEC_Day()");
            var strDate1;
            var strDate2;
            strDate1 = $("#<%=dteEC_Last_Meter_Reading.FindControl("txtDate").ClientID %>").val();
            strDate2 = $("#<%=dteEC_Current_Meter_Reading.FindControl("txtDate").ClientID %>").val();
            //alert("strDate1:" + strDate1 + ";strDate2:" + strDate2);

            var oDate1 = new Date(strDate1);
            var oDate2 = new Date(strDate2);
            //alert("oDate1:" + oDate1 + ";oDate2:" + oDate2);
            var intDay = parseInt(Math.abs(oDate2 - oDate1) / 1000 / 60 / 60 / 24);
            $("#<%=txtEC_Days.ClientID %>").val(intDay);
        }

        //天數
        function EC_Day() {
            //var vIsErr = false;
            //var strValue;
            //天數
            //strValue = $("#<%=txtEC_Days.ClientID %>").val();
            //if (strValue != "") {
            //    strValue = $("#<%=txtEC_Days.ClientID %>").val();
            //    if (!CheckInt(strValue)) {
            //        $("#<%=txtEC_Days.ClientID %>").val('');
            //        alert('「天數」請輸入正確數字');
            //    }
            //}
            CheckNumber();
            CalDaily_Amount();
            CalDuarantee_Rate();
            CalDaily_Billing();
            Check_Amount();
            roundAllDecimal();
        }

        //電費計算金額
        function Calculation_Amt() {
            CheckNumber();
            CalDaily_Amount();
            roundAllDecimal();
        }

        //抄錶紀錄公司購電-度數
        function Meter_Record() {
            getEC_Day();
            CheckNumber();
            CalDuarantee_Rate();
            CalAdditionData();
            roundAllDecimal();

        }
        function CalAdditionData() {
            //txtEC_Meter_Record
            //txtEC_Calculation_Rate
            //3.電費計算金額(應付款金額) = 抄表紀錄 公司購電 - 度數 * 電費計算費率
            //3.電費計算金額(應付款金額) = 電費計算 計費度數 - 度數 * 電費計算費率 20230328 要求修改
            var EC_Calculation_Amt = $("#<%=txtEC_Calculation_Record.ClientID %>").val() * $("#<%=txtEC_Calculation_Rate.ClientID %>").val();
            EC_Calculation_Amt = Math.round((EC_Calculation_Amt + Number.EPSILON) * 1000) / 1000;
            $("#<%=txtEC_Calculation_Amt.ClientID %>").val(EC_Calculation_Amt);

            //6.新增營業稅欄位 = 電費計算金額 * 5 %
            var BusinessTax = EC_Calculation_Amt * $("#<%=txtCBDTaxRate.ClientID %>").val() / 100;
            BusinessTax = Math.round((BusinessTax + Number.EPSILON) * 1000) / 1000;
            $("#<%=txtBusinessTax.ClientID %>").val(BusinessTax);

            //7.發票金額=電費計算金額(應付款金額)+營業稅
            var InvoicePrice = EC_Calculation_Amt + BusinessTax;
            $("#<%=txtInvoicePrice.ClientID %>").val(InvoicePrice);

            //8.應付總金額=發票金額-電表租費
            var TotalPrice = InvoicePrice - $("#<%=txtMeterRent.ClientID %>").val();
            $("#<%=txtTotalPrice.ClientID %>").val(TotalPrice);

            //9.實際入帳金額 = 發票金額 - 手續費(匯費)
            var ActualIncome = InvoicePrice - $("#<%=txtRemittanceFee.ClientID %>").val();
            $("#<%=txtActualIncome.ClientID %>").val(ActualIncome);

            //日平均 購電度數/天數/案場KW 
            var dayAvg = $("#<%=txtEC_Meter_Record.ClientID %>").val() / $("#<%=txtEC_Days.ClientID %>").val() / $("#<%=txtKW.ClientID %>").val();
            dayAvg = Math.round((dayAvg + Number.EPSILON) * 1000) / 1000;
            $("#<%=txtDayAvg.ClientID %>").val(dayAvg);
        }

        //電費計算計費度數
        function Calculation_Record() {
            CheckNumber();
            CalDaily_Billing();
            Check_Amount();
            CalAdditionData();
            roundAllDecimal();
        }

        //電費計算費率 
        function Calculation_Rate() {
            CheckNumber();
            CalDaily_Billing();

            CalAdditionData();
            roundAllDecimal();
        }

        //日金額 = 電費計算金額  /  天數
        function CalDaily_Amount() {
            var strCalValue;
            if ($("#<%=txtEC_Calculation_Amt.ClientID %>").val() != "" && $("#<%=txtEC_Days.ClientID %>").val() != "") {
                strCalValue = $("#<%=txtEC_Calculation_Amt.ClientID %>").val() / $("#<%=txtEC_Days.ClientID %>").val();
                strCalValue = financial(strCalValue, 3);
                //alert("strCalValue:" + strCalValue);
                $("#<%=txtEC_Daily_Amount.ClientID %>").val(strCalValue);
            }
        }

        //日保發度數 =  (抄錶紀錄公司購電-度數)  /  天數
        function CalDuarantee_Rate() {
            var strCalValue;
            if ($("#<%=txtEC_Meter_Record.ClientID %>").val() != "" && $("#<%=txtEC_Days.ClientID %>").val() != "") {
                strCalValue = $("#<%=txtEC_Meter_Record.ClientID %>").val() / $("#<%=txtEC_Days.ClientID %>").val();
                strCalValue = financial(strCalValue, 3);
                //alert("strCalValue:" + strCalValue);
                $("#<%=txtEC_Duarantee_Rate.ClientID %>").val(strCalValue);
            }
        }

        //日計費度數 =  電費計算 計費度數   /   天數
        function CalDaily_Billing() {
            var strCalValue;
            if ($("#<%=txtEC_Calculation_Record.ClientID %>").val() != "" && $("#<%=txtEC_Days.ClientID %>").val() != "") {
                strCalValue = $("#<%=txtEC_Calculation_Record.ClientID %>").val() / $("#<%=txtEC_Days.ClientID %>").val();
                strCalValue = financial(strCalValue, 3);
                //alert("日計費度數:" + strCalValue);
                $("#<%=txtEC_Daily_Billing.ClientID %>").val(strCalValue);
            }
        }

        //驗算 =  日計費度數  *  電費計算費率
        function Check_Amount() {
            var strCalValue;
            if ($("#<%=txtEC_Daily_Billing.ClientID %>").val() != "" && $("#<%=txtEC_Calculation_Rate.ClientID %>").val() != "") {
                strCalValue = $("#<%=txtEC_Daily_Billing.ClientID %>").val() * $("#<%=txtEC_Calculation_Rate.ClientID %>").val();
                strCalValue = financial(strCalValue, 3);
                //alert("驗算:" + strCalValue);
                $("#<%=txtEC_Check_Amount.ClientID %>").val(strCalValue);
            }
        }

        function financial(intValue, x) {
            return Number.parseFloat(intValue).toFixed(x);
        }

        function CheckNumber() {
            //alert("CheckNumber");
            debugger;
            //只能輸入數字
            $(".CheckNum").blur(
                function () {
                    //alert("CheckNum:" + $(this).val());
                    if ($(this).val() != "") {
                        objStr = $(this).val();
                        reg = /^[-+]?(0|[1-9][0-9]*)$/;
                        regMatch = reg.test(objStr);
                        if (!regMatch) {
                            alert('請輸入正確數字');
                            $(this).val('');
                            $(this).focus();
                        }
                    }
                }
            );

            //只能輸入數字
            $(".CheckDecimal_1").blur(function () {
                if ($(this).val() != "") {
                    var n = $(this).val().indexOf("-");
                    if (n > 0) {
                        alert("請輸入正確可含小數點數字(ex:11.124)");
                        $(this).val('');
                        $(this).focus();
                    }
                    else if (!CheckNUM($(this).val(), 18, 3)) {
                        alert("請輸入正確可含小數點數字(ex:11.124)");
                        $(this).val('');
                        $(this).focus();
                    }
                }
            });

            //只能輸入數字
            $(".CheckDecimal_2").blur(function () {
                if ($(this).val() != "") {
                    var n = $(this).val().indexOf("-");
                    if (n > 0) {
                        alert("請輸入正確可含小數點數字(ex:11.1247)");
                        $(this).val('');
                        $(this).focus();
                    }
                    else if (!CheckNUM($(this).val(), 18, 4)) {
                        alert("請輸入正確可含小數點數字(ex:11.1247)");
                        $(this).val('');
                        $(this).focus();
                    }
                }
            });

        }

        function CheckNUM(valueStr, iNum, fNum) {
            var vIsErr = false;
            try {
                var a = parseFloat(valueStr);
                if (isNaN(a)) { vIsErr = true; }
                else {
                    try {
                        var NumArr = valueStr.split(".");

                        if (NumArr[0].length > iNum) {
                            vIsErr = true;
                        }
                        if (NumArr.length == 2) {
                            if (NumArr[0].length > 1 && NumArr[0][0] == '0')
                                vIsErr = true;

                            if (NumArr[1].length > fNum || fNum == 0) {
                                vIsErr = true;
                            }
                        }
                        else {
                            //                            if (valueStr[0] == '0')
                            //                                vIsErr = true;
                        }

                        //alert("vIsErr:" + vIsErr);
                    } catch (fErr) { vIsErr = true; }
                }
            } catch (err) { vIsErr = true; }
            return !vIsErr;
        }

        function roundDecimal(tar) {
            $(tar).val(Math.round($(tar).val()))
        }
        function roundAllDecimal() {
            roundDecimal("#<%=txtEC_Calculation_Amt.ClientID %>");
            roundDecimal("#<%=txtEC_Daily_Amount.ClientID %>");
            roundDecimal("#<%=txtTotalPrice.ClientID %>");
            roundDecimal("#<%=txtActualIncome.ClientID %>");
            roundDecimal("#<%=txtInvoicePrice.ClientID %>");
            roundDecimal("#<%=txtBusinessTax.ClientID %>");
        }

    </script>

</asp:Content>
