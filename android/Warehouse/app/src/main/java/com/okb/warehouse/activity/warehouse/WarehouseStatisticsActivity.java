package com.okb.warehouse.activity.warehouse;

import android.support.design.widget.TabLayout;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;

import android.widget.TextView;

import com.anychart.core.Base;
import com.okb.warehouse.R;
import com.okb.warehouse.activity.adapter.SectionPageAdapter;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.activity.statisticsFragments.CapacityFragment;
import com.okb.warehouse.activity.statisticsFragments.ProductOverviewFragment;

public class WarehouseStatisticsActivity extends BaseActivity {
    private SectionPageAdapter spAdapter;
    private ViewPager viewPager;

    private int warehouseId;
    private String warehouseName;
    private int warehouseCapacity;

    @Override
    protected void onCreate(Bundle savedInstanceBundle){
        super.onCreate(savedInstanceBundle);
        setContent(R.layout.activity_warehouse_statistics);
        warehouseId = this.getIntent().getIntExtra("wId", 0);
        warehouseName = this.getIntent().getStringExtra("wName");
        warehouseCapacity = this.getIntent().getIntExtra("wCapacity", 0);
        getSupportActionBar().setTitle(warehouseName + " Statistics");

        spAdapter = new SectionPageAdapter(getSupportFragmentManager());

        viewPager = findViewById(R.id.container);
        setupViewPager(viewPager);

        TabLayout tabLayout = findViewById(R.id.tabs);
        tabLayout.setupWithViewPager(viewPager);
    }


    private void setupViewPager(ViewPager viewPager){
        SectionPageAdapter adapter = new SectionPageAdapter(getSupportFragmentManager());
        adapter.addFragment(CapacityFragment.newInstance(sp.getString("token", null), warehouseId), "Capacity");
        adapter.addFragment(ProductOverviewFragment.newInstance(sp.getString("token", null), warehouseId, warehouseCapacity), "Product Overview");
        viewPager.setAdapter(adapter);
    }


}
