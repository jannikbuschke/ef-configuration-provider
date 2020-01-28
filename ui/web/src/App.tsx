import React from 'react';
import './App.css';
import { Layout, Menu, Breadcrumb, Icon, Alert } from 'antd';
import { DataTable } from './table';
import { Route, BrowserRouter, Link } from 'react-router-dom';
import { StronglyTypedOptions, getJson } from './strongly-typed-options';
import useSWR from 'swr';

const { Header, Content, Footer, Sider } = Layout;

interface GeneratedControllerAttribute {
  route: string;
  title: string;
}

function App() {
  const { data, error } = useSWR<GeneratedControllerAttribute[]>(
    '/api/__configuration/partial-configurations',
    getJson
  );
  return (
    <BrowserRouter basename={"__configuration"}>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider>
          <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
            <Menu.Item key="1">
              <Link to="/all">
                <Icon type="pie-chart" />
                <span>All</span>
              </Link>
            </Menu.Item>
            {data &&
              data.map(v => (
                <Menu.Item key={v.route + v.title}>
                  <Link to={'/' + v.route}>
                    <Icon type="pie-chart" />
                    <span>{v.title}</span>
                  </Link>
                </Menu.Item>
              ))}
          </Menu>
        </Sider>
        <Layout>
          <Header style={{ background: '#fff', padding: 0 }} />
          <Content style={{ margin: '0 16px' }}>
            <Breadcrumb style={{ margin: '16px 0' }}></Breadcrumb>
            {error && (
              <Alert type="error" banner={true} message={error.toString()} />
            )}
            <div>
              <Route path="/all">
                <DataTable />
              </Route>
              {data &&
                data.map(v => (
                  <Route path={'/' + v.route}>
                    <StronglyTypedOptions path={v.route} title={v.title} />
                  </Route>
                ))}
            </div>
          </Content>
          <Footer style={{ textAlign: 'center' }}>Configuration</Footer>
        </Layout>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
