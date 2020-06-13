import { Component, OnInit, Injector } from '@angular/core';
import { TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBase } from '../widget-component-base';


class DashboardTopStats extends DashboardChartBase {

  totalProfit = 0; totalProfitCounter = 0;
  newFeedbacks = 0; newFeedbacksCounter = 0;
  newOrders = 0; newOrdersCounter = 0;
  newUsers = 0; newUsersCounter = 0;
  employees=0; employeesCounter = 0;
  machines=0; machinesCounter = 0;
  employeesPermits=0; employeesPermitsCounter = 0;
  employeesOfiicialTasks=0; employeesOfiicialTasksPermitsCounter = 0;

  totalProfitChange = 76; totalProfitChangeCounter = 0;
  newFeedbacksChange = 85; newFeedbacksChangeCounter = 0;
  newOrdersChange = 45; newOrdersChangeCounter = 0;
  newUsersChange = 57; newUsersChangeCounter = 0;

  init(totalProfit, newFeedbacks, newOrders, newUsers,employees,machines,employeesPermits,employeesOfiicialTasks) {
    this.totalProfit = totalProfit;
    this.newFeedbacks = newFeedbacks;
    this.newOrders = newOrders;
    this.newUsers = newUsers;
    this.employees = employees;
    this.machines = machines;
    this.employeesPermits = employeesPermits;
    this.employeesOfiicialTasks = employeesOfiicialTasks;
    this.hideLoading();
  }
}

@Component({
  selector: 'app-widget-top-stats',
  templateUrl: './widget-top-stats.component.html',
  styleUrls: ['./widget-top-stats.component.css']
})
export class WidgetTopStatsComponent extends WidgetComponentBase implements OnInit {

  dashboardTopStats: DashboardTopStats;

  constructor(injector: Injector,
    private _tenantDashboardServiceProxy: TenantDashboardServiceProxy
  ) {
    super(injector);
    this.dashboardTopStats = new DashboardTopStats();
  }

  ngOnInit() {
    this.loadTopStatsData();
  }

  loadTopStatsData() {
    this._tenantDashboardServiceProxy.getTopStats().subscribe((data) => {
      this.dashboardTopStats.init(data.totalProfit, data.newFeedbacks, data.newOrders, data.newUsers , data.totalEmployeesCount , data.totalMachinesCount , data.totalEmployeePermits ,  data.totalEmployeeOfficialTasks);
    });
  }
}
