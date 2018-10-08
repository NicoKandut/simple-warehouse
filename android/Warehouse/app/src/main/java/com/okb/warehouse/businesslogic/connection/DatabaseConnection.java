package com.okb.warehouse.businesslogic.connection;

import com.okb.warehouse.businesslogic.data.User;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class DatabaseConnection {
    private static String connString = "jdbc:oracle:thin:@212.152.179.117:1521:ora11g";
    private static String USER = "d5a07";
    private static String PASSWD = "d5a";
    Connection conn = null;

    public DatabaseConnection()throws Exception{
        createConnection();
    }

    private void createConnection() throws Exception{
        if (conn == null){
            DriverManager.registerDriver(new oracle.jdbc.driver.OracleDriver());
            conn = DriverManager.getConnection(connString, USER, PASSWD);
            conn.setAutoCommit(false);
            conn.setTransactionIsolation(Connection.TRANSACTION_READ_COMMITTED);
        }
    }



    private void close(){
        try {
            if (!conn.isClosed()){
                conn.close();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void select(String table){
        try {
            String sql = "SELECT * from table";
            PreparedStatement prest = conn.prepareStatement(sql);
            //prest.setInt(1,1980);
            //prest.setInt(2,2004);
            ResultSet rs = prest.executeQuery();
            prest.close();
            this.close();
        } catch (SQLException s) {
            System.out.println("SQL statement is not executed!");
        }
    }


    public User login(String username, String password){
        User logedin = null;
        ResultSet rs;
        try {
            String sql = "select * from sw_owner where username like '"+ username+"' and password like '" + password + "'";
            PreparedStatement prestmt = conn.prepareStatement(sql);
            rs = prestmt.executeQuery();
            logedin = new User(rs.getString(0), rs.getString(1), rs.getString(2));

            prestmt.close();
            this.close();
        } catch (SQLException s) {
            throw new Exception("SQL statement is not executed!");
        }
        finally{
            return logedin;
        }
    }

    public void register(String username, String password){

    }
}
