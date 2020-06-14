import * as React from "react"
import { Card, Button } from "antd"

export function TestModule() {
  return (
    <Card
      title="CARD TITLE"
      bordered={true}
      size="small"
      extra={<a href="#">More</a>}
      style={{ width: 300 }}
    >
      <p>Hello World from module</p>
      <Button type="primary">Hello</Button>
    </Card>
  )
}
