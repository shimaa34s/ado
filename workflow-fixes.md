# Workflow Component Fixes

## 1. Fix FlowNodeLocal and FlowConnectionLocal interfaces

```typescript
// Updated interfaces - remove extends to avoid missing property errors
interface FlowNodeLocal {
  id: string;
  type: string;
  position: { x: number; y: number };
  data: {
    label: string;
    bodyText: string;
    type: string;
  };
}

interface FlowConnectionLocal {
  id: string;
  sourceId: string;
  targetId: string;
  isDashed?: boolean;
  label?: string;
}
```

## 2. Fix the convertToFlowFormat method

```typescript
private convertToFlowFormat(workflow: WorkflowNode[]): {
  nodes: FlowNodeLocal[];
  connections: FlowConnectionLocal[];
} {
  const nodes: FlowNodeLocal[] = [];
  const connections: FlowConnectionLocal[] = [];
  let xPosition = 0;
  let yPosition = 0;
  const verticalSpacing = 150;

  const processNode = (node: WorkflowNode, parentId?: string) => {
    const flowNode: FlowNodeLocal = {
      id: node.id,
      type: node.type,
      position: { x: xPosition, y: yPosition },
      data: {
        label: node.name,
        bodyText: node.bodyText,
        type: node.type,
      },
    };

    nodes.push(flowNode);
    xPosition += 250;

    if (parentId) {
      connections.push({
        id: `conn-${parentId}-${node.id}`,
        sourceId: parentId,
        targetId: node.id,
      });
    }

    if (node.childrens && node.childrens.length > 0) {
      yPosition += verticalSpacing;
      const childXStart = xPosition - (node.childrens.length * 250) / 2;
      let childX = childXStart;

      node.childrens.forEach((child) => {
        processNode(child, node.id);
        childX += 250;
      });

      yPosition -= verticalSpacing;
    }

    if (node.goto) {
      connections.push({
        id: `conn-goto-${node.id}-${node.goto}`,
        sourceId: node.id,
        targetId: node.goto,
        isDashed: true,
        label: 'GoTo',
      });
    }
  };

  workflow.forEach((node) => processNode(node));
  return { nodes, connections };
}
```

## 3. Fix event handlers

```typescript
onFlowNodeClick(event: any) {
  // Extract node data from the event
  const nodeData = event.data || event.node?.data;
  if (nodeData && nodeData.id) {
    console.log('Node clicked:', nodeData);
    this.selectedElement = this.findElementById(this.mainWorkflow, nodeData.id);
  }
}

onFlowConnectionClick(event: any) {
  // Extract connection data from the event
  const connectionData = event.data || event.connection?.data;
  if (connectionData) {
    console.log('Connection clicked:', connectionData);
  }
}
```

## 4. Updated template section for FFlow

Replace the FFlow template section with:

```html
<!-- عرض مخطط التدفق -->
<div class="flow-container" *ngIf="showFlowView">
  <f-flow>
    <!-- Add nodes using f-node directive -->
    <f-node 
      *ngFor="let node of flowNodes" 
      [fId]="node.id"
      [fPosition]="node.position"
      (fNodeClick)="onFlowNodeClick($event)"
      class="custom-flow-node"
      [ngClass]="'node-type-' + node.data.type">
      
      <div class="node-icon">
        <i [class]="getNodeIcon(node.data.type)"></i>
      </div>
      <div class="node-content">
        <div class="node-title">{{ node.data.label }}</div>
        <div class="node-body" *ngIf="node.data.bodyText">
          {{ truncate(node.data.bodyText, 30) }}
        </div>
      </div>
    </f-node>

    <!-- Add connections using f-connection directive -->
    <f-connection 
      *ngFor="let conn of flowConnections"
      [fId]="conn.id"
      [fSource]="conn.sourceId"
      [fTarget]="conn.targetId"
      [fIsDashed]="conn.isDashed"
      (fConnectionClick)="onFlowConnectionClick($event)">
      
      <span *ngIf="conn.label" class="connection-label">{{ conn.label }}</span>
    </f-connection>
  </f-flow>

  <button class="toggle-view-btn" (click)="toggleView()">
    {{ "RETURN_TO_TREE_VIEW" | translate }}
  </button>
</div>
```

## 5. Alternative simpler FFlow template (if above doesn't work)

```html
<!-- عرض مخطط التدفق -->
<div class="flow-container" *ngIf="showFlowView">
  <f-flow 
    [fNodes]="flowNodes"
    [fConnections]="flowConnections"
    (fNodeClick)="onFlowNodeClick($event)"
    (fConnectionClick)="onFlowConnectionClick($event)">
  </f-flow>

  <button class="toggle-view-btn" (click)="toggleView()">
    {{ "RETURN_TO_TREE_VIEW" | translate }}
  </button>
</div>
```

## 6. Add CSS for flow view

```scss
.flow-container {
  width: 100%;
  height: 600px;
  border: 1px solid #ddd;
  position: relative;
  
  f-flow {
    width: 100%;
    height: 100%;
  }
}

.custom-flow-node {
  display: flex;
  align-items: center;
  padding: 8px 12px;
  background: white;
  border: 2px solid #007bff;
  border-radius: 8px;
  min-width: 120px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  
  .node-icon {
    margin-right: 8px;
    color: #007bff;
  }
  
  .node-content {
    flex: 1;
    
    .node-title {
      font-weight: bold;
      font-size: 12px;
      margin-bottom: 2px;
    }
    
    .node-body {
      font-size: 10px;
      color: #666;
    }
  }
}

.node-type-section { border-color: #31708f; }
.node-type-button { border-color: var(--primary-color); }
.node-type-list { border-color: #27667B; }
.node-type-flow { border-color: #C599B6; }

.connection-label {
  background: white;
  padding: 2px 4px;
  border-radius: 4px;
  font-size: 10px;
  border: 1px solid #ddd;
}

.toggle-view-btn, .view-toggle-btn {
  position: absolute;
  top: 10px;
  right: 10px;
  padding: 8px 16px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  
  &:hover {
    background: #0056b3;
  }
}
```

## Summary of Changes:

1. **Removed interface extensions** to avoid missing property errors
2. **Fixed connection creation** to use `sourceId`/`targetId` instead of `source`/`target` objects
3. **Updated event handlers** to properly handle FFlow events
4. **Provided two template options** for FFlow integration
5. **Added CSS styling** for the flow view

Choose the template option that works with your version of @foblex/flow library.