package com.okb.warehouse.activity.statisticsFragments;


import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.anychart.AnyChart;
import com.anychart.AnyChartView;
import com.anychart.chart.common.dataentry.DataEntry;
import com.anychart.chart.common.dataentry.ValueDataEntry;
import com.anychart.charts.Cartesian;
import com.anychart.charts.Pie;
import com.anychart.core.annotations.VerticalLine;
import com.anychart.core.cartesian.series.Line;
import com.anychart.core.utils.OrdinalZoom;
import com.anychart.data.Mapping;
import com.anychart.data.Set;
import com.anychart.enums.Anchor;
import com.anychart.enums.MarkerType;
import com.anychart.enums.TooltipPositionMode;
import com.anychart.graphics.vector.Stroke;
import com.okb.warehouse.R;
import com.okb.warehouse.businesslogic.HandleStatisticData;
import com.okb.warehouse.businesslogic.UpdateView;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 */
public class CapacityFragment extends Fragment implements UpdateView {
    private static final String ARG_TOKEN = "argToken";
    private static final String ARG_WID = "argWID";

    private String token;
    private int wid;
    private View v;

    public static CapacityFragment newInstance(String token, int warehouseId) {
        CapacityFragment fragment = new CapacityFragment();
        Bundle args = new Bundle();
        args.putString(ARG_TOKEN, token);
        args.putInt(ARG_WID, warehouseId);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        v = inflater.inflate(R.layout.fragment_linechart, container, false);
        if (getArguments() != null) {
            token = getArguments().getString(ARG_TOKEN);
            wid = getArguments().getInt(ARG_WID);
        }
        HandleStatisticData hsd = HandleStatisticData.newInstance();
        hsd.getHistographyData(token, wid, this);

        return v;
    }

    @Override
    public void createChart(List<DataEntry> entries) {
        Cartesian cartesian = AnyChart.line();
        cartesian.xAxis(0).title("Time");
        cartesian.yAxis(0).title("Amount");

        Line series = cartesian.line(testData());

        AnyChartView anyChartView = v.findViewById(R.id.flc_acv_linechart);
        anyChartView.setChart(cartesian);
    }

    private List<DataEntry> testData(){
        List<DataEntry> data = new ArrayList<>();
        data.add(new ValueDataEntry("00:00", 10));
        data.add(new ValueDataEntry("01:00", 4));
        data.add(new ValueDataEntry("02:00", 17));
        data.add(new ValueDataEntry("03:00", 20));
        data.add(new ValueDataEntry("04:00", 20));
        data.add(new ValueDataEntry("05:00", 16));
        data.add(new ValueDataEntry("06:00", 35));
        data.add(new ValueDataEntry("07:00", 6));
        data.add(new ValueDataEntry("08:00", 15));
        data.add(new ValueDataEntry("09:00", 19));
        data.add(new ValueDataEntry("10:00", 25));
        data.add(new ValueDataEntry("11:00", 34));
        data.add(new ValueDataEntry("12:00", 42));
        data.add(new ValueDataEntry("13:00", 14));
        data.add(new ValueDataEntry("14:00", 50));
        data.add(new ValueDataEntry("15:00", 25));
        data.add(new ValueDataEntry("16:00", 34));
        data.add(new ValueDataEntry("17:00", 40));
        data.add(new ValueDataEntry("18:00", 64));
        data.add(new ValueDataEntry("19:00", 48));
        data.add(new ValueDataEntry("20:00", 22));
        data.add(new ValueDataEntry("21:00", 37));
        data.add(new ValueDataEntry("22:00", 53));
        data.add(new ValueDataEntry("23:00", 62));
        return data;
    }
}
