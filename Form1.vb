Imports MySql.Data.MySqlClient
Public Class Form1
    Dim strkon As String = "server=localhost;uid=root;database=rental_mobil_22"
    Dim kon As New MySqlConnection(strkon)
    Dim perintah As New MySqlCommand
    Dim mda As New MySqlDataAdapter
    Dim ds As New DataSet
    Dim cek As MySqlDataReader
    Dim sewa, total As Double

    Sub tidakaktif()
        Txtnopin.Enabled = False
        dtppinjam.Enabled = False
        txtnapem.Enabled = False
        txtnomobil.Enabled = False
        cmbjenis.Enabled = False
        txtsewa.Enabled = False
        txtlama.Enabled = False
        txttotal.Enabled = False

        Txtnopin.BackColor = Color.Gray
        txtnapem.BackColor = Color.Gray
        txtnomobil.BackColor = Color.Gray
        cmbjenis.BackColor = Color.Gray
        txtsewa.BackColor = Color.Gray
        txtlama.BackColor = Color.Gray
        txttotal.BackColor = Color.Gray

        CmdSimpan.Enabled = False
        CmdBatal.Enabled = False
        CmdHapus.Enabled = False
        CmdUpdate.Enabled = False
    End Sub

    Sub aktif()
        txtnopin.Enabled = True
        dtppinjam.Enabled = True
        txtnapem.Enabled = True
        txtnomobil.Enabled = True
        cmbjenis.Enabled = True
        txtsewa.Enabled = True
        txtlama.Enabled = True
        txttotal.Enabled = True
        txtnopin.BackColor = Color.White
        txtnapem.BackColor = Color.White
        txtnomobil.BackColor = Color.White
        cmbjenis.BackColor = Color.White
        txtsewa.BackColor = Color.White
        txtlama.BackColor = Color.White
        txttotal.BackColor = Color.White
        CmdSimpan.Enabled = True
        CmdBatal.Enabled = True
        CmdHapus.Enabled = True
        CmdUpdate.Enabled = True
    End Sub


    Sub bersih()
        Txtnopin.Text = ""
        dtppinjam.Text = ""
        txtnapem.Text = ""
        txtnomobil.Text = ""
        cmbjenis.Text = ""
        txtsewa.Text = ""
        txtlama.Text = ""
        txttotal.Text = ""
    End Sub

    Sub tampil()
        kon.Open()
        perintah.Connection = kon
        perintah.CommandType = CommandType.Text
        perintah.CommandText = "select * from peminjaman"
        mda.SelectCommand = perintah
        ds.Tables.Clear()
        mda.Fill(ds, "peminjaman")
        DgTampil.DataSource = ds.Tables("peminjaman")
        kon.Close()
    End Sub

    Private Sub Txtnopin_TextChanged(sender As Object, e As KeyEventArgs) Handles Txtnopin.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                kon.Open()
                perintah.Connection = kon
                perintah.CommandType = CommandType.Text
                perintah.CommandText = "select * from peminjaman " &
                " where Nopinjam='" & Txtnopin.Text & "'"
                cek = perintah.ExecuteReader
                cek.Read()
                MsgBox("data sudah ada...!", MsgBoxStyle.Information, "Pesan")
                If cek.HasRows Then
                    dtppinjam.Value = cek.Item("Tglpinjam")
                    txtnapem.Text = cek.Item("Namapeminjam")
                    txtnomobil.Text = cek.Item("Nomobil")
                    cmbjenis.Text = cek.Item("Jenismobil")
                    txtsewa.Text = cek.Item("Sewa")
                    txtlama.Text = cek.Item("lama")
                    txttotal.Text = cek.Item("Totalbayar")
                    CmdSimpan.Enabled = False
                End If
                kon.Close()
                ' tidakaktif()
                CmdTambah.Enabled = True
        End Select

    End Sub

    Private Sub cmbjenis_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbjenis.SelectedIndexChanged
        Select Case cmbjenis.SelectedIndex
            Case 0
                sewa = 250000
            Case 1
                sewa = 250000
            Case 2
                sewa = 300000
            Case 3
                sewa = 700000
            Case 4
                sewa = 1000000
        End Select
        txtsewa.Text = Format(sewa, "Rp ###,###,##")
    End Sub

    Private Sub CmdTambah_Click(sender As Object, e As EventArgs) Handles CmdTambah.Click
        aktif()
        Txtnopin.Focus()
        CmdTambah.Enabled = False

    End Sub

    Private Sub txtlama_TextChanged(sender As Object, e As EventArgs) Handles txtlama.TextChanged
        total = sewa * Val(txtlama.Text)
        txttotal.Text = Format(total, "Rp ###,###,##")
    End Sub

    Private Sub CmdSimpan_Click(sender As Object, e As EventArgs) Handles CmdSimpan.Click
        kon.Open()
        perintah.Connection = kon
        perintah.CommandType = CommandType.Text
        perintah.CommandText = "insert into peminjaman values " &
        " ('" & Txtnopin.Text & "','" & Format(dtppinjam.Value, "yyyy-MM-dd") & "', " &
        " '" & txtnapem.Text & "','" & txtnomobil.Text & "', " &
       " '" & cmbjenis.Text & "','" & sewa & "','" & txtlama.Text & "', " &
       " '" & total & "')"
        perintah.ExecuteNonQuery()
        kon.Close()
        MsgBox("data berhasil disimpan", MsgBoxStyle.Information, "Pesan")
        tampil()
        bersih()
        tidakaktif()
        CmdTambah.Enabled = True
    End Sub

    Private Sub CmdBatal_Click(sender As Object, e As EventArgs) Handles CmdBatal.Click
        tidakaktif()
        CmdTambah.Enabled = True
        bersih()
    End Sub

    Private Sub CmdUpdate_Click(sender As Object, e As EventArgs) Handles CmdUpdate.Click
        kon.Open()
        perintah.Connection = kon
        perintah.CommandType = CommandType.Text
        perintah.CommandText = "update peminjaman set Tglpinjam='" & Format(dtppinjam.Value,
"yyyy-MM-dd") & "', " &
" Namapeminjam='" & txtnapem.Text & "', Nomobil='" & txtnomobil.Text & "', " &
" Jenismobil='" & cmbjenis.Text & "',Sewa='" & sewa & "',lama='" & txtlama.Text & "', " &
" Totalbayar='" & total & "' where Nopinjam='" & Txtnopin.Text & "'"
        perintah.ExecuteNonQuery()
        kon.Close()
        tampil()
        bersih()
        tidakaktif()

    End Sub

    Private Sub CmdHapus_Click(sender As Object, e As EventArgs) Handles CmdHapus.Click
        kon.Open()
        perintah.Connection = kon
        perintah.CommandType = CommandType.Text
        perintah.CommandText = "delete from Peminjaman where Nopinjam='" & Txtnopin.Text & "'"
        perintah.ExecuteNonQuery()
        kon.Close()
        tampil()
        bersih()
    End Sub

    Private Sub CmdKeluar_Click(sender As Object, e As EventArgs) Handles CmdKeluar.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tidakaktif()
        bersih()
        tampil()
    End Sub


End Class
