// Add these updated methods to your WorkflowComponent class

// Updated interfaces to match @foblex/flow
interface FlowNodeData {
  id: string;
  type: string;
  position: { x: number; y: number };
  size?: { width: number; height: number };
  data: {
    label: string;
    bodyText: string;
    type: string;
    node: WorkflowNode;
  };
}

interface FlowConnectionData {
  id: string;
  sourceId: string;
  targetId: string;
  isDashed?: boolean;
  label?: string;
}

// Updated event handlers for @foblex/flow
onFlowNodeSelect(event: any): void {
  console.log('Flow node select event:', event);
  
  // The event structure might vary, so we need to handle different cases
  let nodeId: string | null = null;
  
  if (event?.fNodeId) {
    nodeId = event.fNodeId;
  } else if (event?.id) {
    nodeId = event.id;
  } else if (event?.target?.getAttribute) {
    nodeId = event.target.getAttribute('fNodeId');
  }
  
  if (nodeId) {
    const flowNode = this.flowNodes.find(n => n.id === nodeId);
    if (flowNode?.data?.node) {
      this.selectedElement = flowNode.data.node;
      this.selectedFlowNode = flowNode;
      console.log('Selected workflow element:', this.selectedElement);
    }
  }
}

onFlowConnectionClick(event: any): void {
  console.log('Connection clicked:', event);
  
  let connectionId: string | null = null;
  
  if (event?.fConnectionId) {
    connectionId = event.fConnectionId;
  } else if (event?.id) {
    connectionId = event.id;
  }
  
  if (connectionId) {
    const connection = this.flowConnections.find(c => c.id === connectionId);
    console.log('Selected connection:', connection);
  }
}

// Updated convertToFlowFormat method
private convertToFlowFormat(workflow: WorkflowNode[]): {
  nodes: FlowNodeData[];
  connections: FlowConnectionData[];
} {
  const nodes: FlowNodeData[] = [];
  const connections: FlowConnectionData[] = [];
  const nodePositions = new Map<string, { x: number; y: number }>();

  let currentX = 100;
  let currentY = 100;
  const horizontalSpacing = 250;
  const verticalSpacing = 150;

  const processNode = (
    node: WorkflowNode,
    parentId?: string,
    level: number = 0,
    indexAtLevel: number = 0
  ): void => {
    // Calculate position
    const nodePosition = {
      x: currentX + indexAtLevel * horizontalSpacing,
      y: currentY + level * verticalSpacing,
    };

    // Create flow node with correct structure for @foblex/flow
    const flowNode: FlowNodeData = {
      id: node.id,
      type: 'workflow-node',
      position: nodePosition,
      size: { width: 180, height: 100 },
      data: {
        label: this.translate.instant(node.name) || node.name,
        bodyText: node.bodyText || '',
        type: node.type,
        node: node,
      },
    };

    nodes.push(flowNode);
    nodePositions.set(node.id, nodePosition);

    // Create connection from parent
    if (parentId) {
      const connection: FlowConnectionData = {
        id: `conn-${parentId}-${node.id}`,
        sourceId: parentId,
        targetId: node.id,
      };
      connections.push(connection);
    }

    // Process children
    if (node.childrens && node.childrens.length > 0) {
      node.childrens.forEach((child, index) => {
        processNode(child, node.id, level + 1, index);
      });
    }

    // Handle goto connections (dashed line)
    if (node.goto) {
      const gotoConnection: FlowConnectionData = {
        id: `conn-goto-${node.id}-${node.goto}`,
        sourceId: node.id,
        targetId: node.goto,
        isDashed: true,
        label: 'GoTo',
      };
      connections.push(gotoConnection);
    }
  };

  // Process root level nodes
  workflow.forEach((rootNode, index) => {
    processNode(rootNode, undefined, 0, index);
  });

  return { nodes, connections };
}