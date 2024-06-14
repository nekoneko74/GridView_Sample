using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GridView_Sample
{
    /// <summary>
    /// GridViewの操作サンプル
    /// </summary>
    public partial class GridViewForm0 : System.Web.UI.Page
    {
        /// <summary>
        /// GridViewのページ変更イベントが発生した際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Literal_Event.Text = "PageIndexChanging";
            this.Literal_Args.Text = $"e.NewPageIndex: {e.NewPageIndex}";
        }

        /// <summary>
        /// GridViewの行にデータがバインドされる際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // データ行をバインドする
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                // 表示される文字列を書き換える
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                DataRow rowData = rowView.Row;
                e.Row.Cells[5].Text = (9 == (Byte)rowData.ItemArray[4]) ? "システム管理者" : "一般スタッフ";
            }
        }

        /// <summary>
        /// GridViewの行が削除されようとしているとき（「削除」リンクがクリックされた際）に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.Literal_Event.Text = "RowDeleting";
            this.Literal_Args.Text = $"e.RowIndex: {e.RowIndex}, GridView.DataKeys[e.RowIndex].Value: {this.GridView.DataKeys[e.RowIndex].Value}";

            // 標準の削除操作をキャンセルする
            e.Cancel = true;
        }

        /// <summary>
        /// GridViewの行が編集されようとしているとき（「編集」リンクがクリックされた際）に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.Literal_Event.Text = "RowEditing";
            this.Literal_Args.Text = $"e.NewEditIndex: {e.NewEditIndex}, GridView.DataKeys[e.NewEditIndex].Value: {this.GridView.DataKeys[e.NewEditIndex].Value}";

            // 標準の編集操作をキャンセルする
            e.Cancel = true;
        }

        /// <summary>
        /// GridViewの「選択状態」が変更された後に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Literal_Event.Text = "SelectedIndexChanged";
            this.Literal_Args.Text = $"GridView.SelectedIndex: {this.GridView.SelectedIndex}, GridView.SelectedDataKey.Value: {this.GridView.SelectedDataKey.Value}";
        }

        /// <summary>
        /// GridViewの「選択状態」が変更されようとしているとき（「選択」リンクがクリックされた際）に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.Literal_Event.Text = "SelectedIndexChanging";
            this.Literal_Args.Text = $"e.NewSelectedIndex: {e.NewSelectedIndex}, GridView.DataKeys[e.NewSelectedIndex].Value: {this.GridView.DataKeys[e.NewSelectedIndex].Value}";
        }

        /// <summary>
        /// ページがロードされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}