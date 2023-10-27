    function OpenPopup() {         
//            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=700,height=500");
        //            return false;
        var left = (screen.width - 600) / 2; //370
        var top = (screen.height - 600) / 2;
        //window.open("../../earchDlgV.Aspx", "List", "scrollbars=yes,resizable=no,width=800,height=500");
        window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 650 + ', height=' + 600 + ', top=' + top + ', left=' + left);
        return false;
        }       
    
    function OpenSearchDlg() {               
//            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=800,height=500");
        //            return false;
        var left = (screen.width - 1200) / 2; //370
        var top = (screen.height - 600) / 2;
        //window.open("../../earchDlgV.Aspx", "List", "scrollbars=yes,resizable=no,width=800,height=500");
        window.open("../../earchDlgV.Aspx", "", 'menubar=no, scrollbars=yes, resizable=yes, width=' + 1200 + ', height=' + 600 + ', top=' + top + ', left=' + left);
        return false;
    }


    function OpenSearchGrid() {
        var left = (screen.width - 800) / 2; //370
        var top = (screen.height - 600) / 2;
        //window.open("../../earchDlgV.Aspx", "List", "scrollbars=yes,resizable=no,width=800,height=500");
        window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 800 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }


        function FindMultiDlg() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            window.open("../../FindMultiDlg.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }

        function FindDlg() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            window.open("../../FindDlg.Aspx", "", 'toolbar=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }
    
    function openprintdlg()
        {
            var wOpen;
            //            wOpen = window.open("../../Rpt/PrintForm.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintForm.Aspx", '_blank');
            //wOpen.print();                  
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false;
        }


        
    function openprintdlgms()
        {
            var wOpen;
            // wOpen = window.open("../../Rpt/PrintFormMs.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintFormMs.Aspx", '_blank');           
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }
    
    function openprintdlg3ds()
        {
            var wOpen;
            //wOpen = window.open("../../Rpt/PrintForm3.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintForm3.Aspx", '_blank');          
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }
        
    function openprintdlg2ds()
        {
            var wOpen;
            //wOpen = window.open("../../Rpt/PrintForm2.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintForm2.Aspx", '_blank');           
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }        
        
    function openprintdlg4ds()
        {
            var wOpen;
            //wOpen = window.open("../../Rpt/PrintForm4.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintForm4.Aspx", '_blank');        
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }
        
    function openprintdlg7ds()
        {
            var wOpen;
            // wOpen = window.open("../../Rpt/PrintForm7.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen = window.open("../../Rpt/PrintForm7.Aspx", '_blank');              
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }
    
    function OpenSearchMultiDlgAll() {    
    window.open("../../SearchMultiDlgAll.Aspx","List","scrollbars=1,resizable=0,width=780,height=500");
    return false;
    } 
    
    function OpenSearchMultiDlg() {
//        window.open("../../SearchMultiDlg.Aspx","List","scrollbars=1,resizable=0,width=780,height=500");
//        return false;

        var left = (screen.width - 1200) / 2; //370
        var top = (screen.height - 800) / 2;
        window.open("../../SearchMultiDlg.Aspx", "", 'toolbar=no, location=no, menubar=no, scrollbars=yes, resizable=yes, width=' + 1200 + ', height=' + 600 + ', top=' + top + ', left=' + left);
        return false;
    } 
    
    function OpenSearchMultiDlg2() {        
        window.open("../../SearchMultiDlg2.Aspx","List","scrollbars=yes,resizable=no,width=700,height=500");        
        return false;
    } 
    
    
    
    function OpenAssign(_groupBy, _keyid, _code, _form) {         
            //alert("../../Assign/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code+"&GroupBy="+_groupBy);
            window.open("../../Assign/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code+"&GroupBy="+_groupBy,"List","scrollbars=yes,resizable=no,width=700,height=500");        
            return false;
    }   
        
    function OpenFilterCriteria() {         
            window.open("../../UserControl/AdvanceSearch.Aspx","List","scrollbars=yes,resizable=no,width=550,height=400");        
            return false;
    }       
        
    function OpenMaster(_keyid, _prm) {
        //window.open("../../Master/" + _prm + "/" + _prm + ".Aspx?KeyId=" + _keyid + "&ContainerId=" + _prm + "Id", "List", "scrollbars=yes,resizable=no,width=700,height=500");
        window.open("../../Master/" + _prm + "/" + _prm + ".Aspx?KeyId=" + _keyid + "&ContainerId=" + _prm + "Id", '_blank');         
            return false;
    }    
    
    function OpenTransaction(_form, _keyid, _code) {
            window.open("../../Transaction/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code,"List","scrollbars=yes,resizable=no,width=700,height=500");            
            return false;
    }
    
    function OpenTransactionSelf(_form, _keyid, _code) {
            window.open("../../Transaction/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code,"_self","scrollbars=yes,resizable=no,width=700,height=500");            
            return false;
    }