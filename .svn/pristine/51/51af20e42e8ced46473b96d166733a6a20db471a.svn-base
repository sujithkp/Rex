using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecordExplorer.Data;

namespace RecordExplorer.UI
{
    public class AddRecordController
    {
        private static AddRecordForm form;

        public AddRecordDetail GetAddRecordDetail(AddRecordDetail prevDetail)
        {
            form = form ?? new AddRecordForm();

            prevDetail.DataSource.Sort();

            form.DataSource = prevDetail.DataSource;
            form.TableName = prevDetail.TableName;
            form.PKey = prevDetail.PKey;

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return new AddRecordDetail(form.TableName, form.PKey) { IsNew = true };

            if (prevDetail == null) return null;

            prevDetail.IsNew = false;
            return prevDetail;
        }
    }
}
