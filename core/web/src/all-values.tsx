import * as React from "react"
import { Formik } from "formik"
import {
  Table,
  Form,
  AddRowButton,
  RemoveRowButton,
  Input,
  SubmitButton,
} from "formik-antd"
import { Button, Alert, PageHeader, message, Tooltip } from "antd"
import * as dayjs from "dayjs"
import { useQuery } from "react-query"
import { DeleteOutlined } from "@ant-design/icons"

async function post<T>(url: string, payload: T) {
  return fetch(url, {
    method: "POST",
    body: JSON.stringify(payload),
    headers: { "content-type": "application/json" },
  })
}

interface ConfigurationValue {
  name: string
  value: string
}

interface UpdateRequest {
  values: ConfigurationValue[]
}

interface Configuration {
  id: number
  values: { [id: string]: string }
  created: string
  user: string | null
}

export function AllValues() {
  const [postError, setPostError] = React.useState("")
  const url = "/api/__configuration/current"
  const { data, error, refetch: revalidate } = useQuery<Configuration, string>(
    url,
    (key) => fetch(key).then((v) => v.json()),
  )

  const tableData = React.useMemo(() => {
    return data
      ? Object.keys(data.values).map(
          (v) => ({ name: v, value: data.values[v] } as ConfigurationValue),
        )
      : []
  }, [data])
  return (
    <div style={{ background: "#fff", padding: 24 }}>
      {error && (
        <Alert type="warning" banner={true} message={error.toString()} />
      )}
      <div>{postError && <Alert type="error" message={postError} />}</div>
      <Formik<{ tableData: ConfigurationValue[] }>
        initialValues={{ tableData }}
        enableReinitialize={true}
        onSubmit={async (values, actions) => {
          const response = await post("/api/__configuration/update", {
            values: values.tableData,
          } as UpdateRequest)
          actions.setSubmitting(false)
          if (response.ok) {
            message.success("saved")
            setPostError("")
            revalidate()
          } else {
            message.error(response.statusText)
            const error = await response.text()
            setPostError(error)
          }
        }}
      >
        <Form>
          <PageHeader
            title="All"
            subTitle={
              data ? (
                <Tooltip title="last edited">
                  <div>{dayjs(data.created).toString()}</div>
                </Tooltip>
              ) : null
            }
            extra={[
              <AddRowButton
                size="small"
                key={1}
                name="tableData"
                style={{ marginBottom: 20 }}
                createNewRow={() => ({
                  name: "id",
                  value: "value",
                })}
              >
                Add
              </AddRowButton>,
              <Button
                size="small"
                key={2}
                icon="reload"
                onClick={() => revalidate()}
              >
                refresh
              </Button>,
              <SubmitButton size="small" key={3}>
                save
              </SubmitButton>,
            ]}
          ></PageHeader>

          <Table
            loading={!data && !error}
            name="tableData"
            rowKey={(row, index) => "" + index}
            size="middle"
            pagination={false}
            columns={[
              {
                width: 300,
                title: "Name",
                key: "name",
                render: (text, record, i) => (
                  <Input
                    fast={true}
                    style={{ border: "none", background: "transparent" }}
                    name={`tableData.${i}.name`}
                  />
                ),
              },
              {
                title: "Value",
                key: "value",
                render: (text, record, i) => (
                  <Input
                    fast={true}
                    style={{ border: "none", background: "transparent" }}
                    name={`tableData.${i}.value`}
                  />
                ),
              },
              {
                width: 32,
                key: "actions",
                align: "right",
                render: (text, record, index) => (
                  <RemoveRowButton
                    style={{ border: "none", background: "transparent" }}
                    icon={<DeleteOutlined />}
                    name="tableData"
                    index={index}
                  />
                ),
              },
            ]}
          />
        </Form>
      </Formik>
    </div>
  )
}
