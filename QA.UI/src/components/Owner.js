import React from 'react';

import 'devextreme/data/odata/store';
import {
  Column,
  DataGrid,
  FilterRow,
  HeaderFilter,
  GroupPanel,
  Scrolling,
  Editing,
  Grouping,
  Lookup,
  MasterDetail,
  Summary,
  RangeRule,
  RequiredRule,
  StringLengthRule,
  GroupItem,
  TotalItem,
  ValueFormat,
  Pager, Paging 
} from 'devextreme-react/data-grid';
import { createStore } from 'devextreme-aspnet-data-nojquery';

const url = 'https://js.devexpress.com/Demos/Mvc/api/DataGridWebApi';
//const apiurl = 'http://localhost:5000/api';
const apiurl = 'http://azvd6-16q7:5100/api';
const displayModes = [{ text: 'Display Mode \'full\'', value: 'full' }, { text: 'Display Mode \'compact\'', value: 'compact' }];
const allowedPageSizes = [5, 10, 50, 100, 'all'];

const dataSource = createStore({
  key: 'OrderID',
  loadUrl: `${apiurl}/Order/Orders`,
  //loadUrl: `${url}/Orders`,
  insertUrl: `${apiurl}/Order/InsertOrder`,
  updateUrl: `${apiurl}/Order/UpdateOrder`,
  deleteUrl: `${apiurl}/Order/DeleteOrder`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = { withCredentials: false };
  },
});

const customersData = createStore({
  key: 'Value',
  loadUrl: `${apiurl}/Customer/CustomersLookup`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = { withCredentials: false };
  },
});

const shippersData = createStore({
  key: 'Value',
  loadUrl: `${apiurl}/Shipper/ShippersLookup`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = { withCredentials: false };
  },
});

class Owner extends React.Component {
  displayModeChange = (value) => {
    this.setState({ ...this.state, displayMode: value });
  }

  showPageSizeSelectorChange = (value) => {
    this.setState({ ...this.state, showPageSizeSelector: value });
  }

  showInfoChange = (value) => {
    this.setState({ ...this.state, showInfo: value });
  }

  showNavButtonsChange = (value) => {
    this.setState({ ...this.state, showNavButtons: value });
  }

  isCompactMode() {
    return this.state.displayMode === 'compact';
  }

  customizeColumns(columns) {
    columns[0].width = 120;
  }

  constructor(props) {
    super(props);
    this.state = {
      displayMode: 'full',
      showPageSizeSelector: true,
      showInfo: true,
      showNavButtons: true,
    };
  }
  render() {
    return (
      <DataGrid
        id='gridContainer'
        dataSource={dataSource}
        showBorders={true}
        height={700}
        remoteOperations={true}
        keyExpr="id"
        customizeColumns={this.customizeColumns}
      >
        <HeaderFilter visible={true} allowSearch={true} />
        <GroupPanel visible={false} />
        <Scrolling mode="standard" />
        <Paging defaultPageSize={10} />
          <Pager
            visible={true}
            allowedPageSizes={allowedPageSizes}
            displayMode={this.state.displayMode}
            showPageSizeSelector={this.state.showPageSizeSelector}
            showInfo={this.state.showInfo}
            showNavigationButtons={this.state.showNavButtons} />
        <Editing
          mode="row"
          allowAdding={true}
          allowDeleting={true}
          allowUpdating={true}
        />
        <Grouping autoExpandAll={true} />
        <Column dataField="CustomerID" caption="Custoemr">
          <Lookup dataSource={customersData} valueExpr="Value" displayExpr="Text" />
          <StringLengthRule max={5} message="The field Customer must be a string with a maximum length of 5." />
        </Column>

        <Column dataField="OrderDate" dataType="date">
          <RequiredRule message="The OrderDate field is required." />
        </Column>

        <Column dataField="Freight" caption="Weight">
          <RangeRule min={0} max={2000} message="The field Freight must be between 0 and 2000." />
        </Column>

        <Column dataField="ShipCountry">
          <StringLengthRule max={15} message="The field ShipCountry must be a string with a maximum length of 15." />
        </Column>

        <Column
          dataField="ShipVia"
          caption="Shipping Company"
          dataType="number"
        >
          <Lookup dataSource={shippersData} valueExpr="Value" displayExpr="Text" />
        </Column>
        <Summary>
          <TotalItem column="Freight" summaryType="sum">
            <ValueFormat type="decimal" precision={2} />
          </TotalItem>

          <GroupItem column="Freight" summaryType="sum">
            <ValueFormat type="decimal" precision={2} />
          </GroupItem>

          <GroupItem summaryType="count" />

        </Summary>
      </DataGrid>
    );
  }
}

export default Owner;