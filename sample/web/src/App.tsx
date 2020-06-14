import React from 'react';
import { Layout, Menu, Breadcrumb, Alert } from 'antd';
import { AllValues } from 'glow-configuration/es/all-values';
import { Route, BrowserRouter, Link, Routes } from 'react-router-dom';
import { StronglyTypedOptions } from 'glow-configuration/es/strongly-typed-options';
import { GeneratedControllerAttribute } from 'glow-configuration';
import { useQuery } from 'react-query';
import 'antd/dist/antd.css';

const { Header, Content, Footer, Sider } = Layout;

function App() {
  const url = '/api/__configuration/partial-configurations';
  const { data, error } = useQuery(url, (key) =>
    fetch(key).then((v) => v.json())
  );
  return (
    <BrowserRouter>
      <Layout style={{ minHeight: '100vh' }}>
        <Sidebar data={data} />
        <Main error={error} data={data} />
      </Layout>
    </BrowserRouter>
  );
}

function Main({
  error,
  data,
}: {
  error: any;
  data?: GeneratedControllerAttribute[];
}) {
  return (
    <Layout>
      <Header style={{ background: '#fff', padding: 0 }} />
      <Content style={{ margin: '0 16px' }}>
        <Breadcrumb style={{ margin: '16px 0' }}></Breadcrumb>
        {error && (
          <Alert type="error" banner={true} message={error.toString()} />
        )}
        <div>
          <Routes>
            <Route path={'/all'} element={<AllValues />} />
            {data &&
              data.map((v) => (
                <Route
                  path={'/' + v.route}
                  element={
                    <StronglyTypedOptions path={v.route} title={v.title} />
                  }
                />
              ))}
          </Routes>
        </div>
      </Content>
      <Footer style={{ textAlign: 'center' }}>Configuration</Footer>
    </Layout>
  );
}

function Sidebar({ data }: { data?: GeneratedControllerAttribute[] }) {
  return (
    <Sider>
      <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
        <Menu.Item key="1">
          <Link to="/all">
            <span>All</span>
          </Link>
        </Menu.Item>
        {data &&
          data.map((v) => (
            <Menu.Item key={v.route + v.title}>
              <Link to={'/' + v.route}>
                <span>{v.title}</span>
              </Link>
            </Menu.Item>
          ))}
      </Menu>
    </Sider>
  );
}

export default App;
