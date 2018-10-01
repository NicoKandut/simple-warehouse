package com.okb.warehouse.businesslogic.connection;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class databaseConnection {
    private Connection connection;

    public void connect() {
        Connection con = null;
        int count = 0;
        try {
            Class.forName("com.mysql.jdbc.Driver");
            con = DriverManager.getConnection
                    ("jdbc:mysql://10.0.2.2:3306/stock", "root", "root");
            try {
                String sql;
                //	  sql
                //	  = "SELECT title,year_made FROM movies WHERE year_made >= ? AND year_made <= ?";
                sql
                        = "SELECT hiduke,jikan,code,price FROM table_stock";
                PreparedStatement prest = con.prepareStatement(sql);
                //prest.setInt(1,1980);
                //prest.setInt(2,2004);
                ResultSet rs = prest.executeQuery();
                while (rs.next()) {
                    hiduke = rs.getString(1);
                    price = rs.getInt(4);
                    count++;
                    System.out.println(hiduke + "\t" + "- " + price);
                }
                System.out.println("Number of records: " + count);
                prest.close();
                con.close();
            } catch (SQLException s) {
                System.out.println("SQL statement is not executed!");
                errmsg = errmsg + s.getMessage();
            }
        } catch (Exception e) {
            e.printStackTrace();
            errmsg = errmsg + e.getMessage();
        }
        handler.sendEmptyMessage(0);
    }
}
