// Add these helper methods to your WorkflowComponent class for the custom flow view

// Zoom functionality
private zoomLevel: number = 1;
private panX: number = 0;
private panY: number = 0;

zoomIn(): void {
  this.zoomLevel = Math.min(this.zoomLevel * 1.2, 3);
  this.updateCanvasTransform();
}

zoomOut(): void {
  this.zoomLevel = Math.max(this.zoomLevel / 1.2, 0.3);
  this.updateCanvasTransform();
}

resetZoom(): void {
  this.zoomLevel = 1;
  this.panX = 0;
  this.panY = 0;
  this.updateCanvasTransform();
}

private updateCanvasTransform(): void {
  const canvas = document.querySelector('.flow-canvas') as HTMLElement;
  if (canvas) {
    canvas.style.transform = `translate(${this.panX}px, ${this.panY}px) scale(${this.zoomLevel})`;
  }
}

// Get connection path for SVG
getConnectionPath(connection: FlowConnectionData): string {
  const sourceNode = this.flowNodes.find(n => n.id === connection.sourceId);
  const targetNode = this.flowNodes.find(n => n.id === connection.targetId);
  
  if (!sourceNode || !targetNode) {
    return '';
  }

  const startX = sourceNode.position.x + 180; // node width
  const startY = sourceNode.position.y + 50;  // node height / 2
  const endX = targetNode.position.x;
  const endY = targetNode.position.y + 50;

  // Create a curved path
  const midX = (startX + endX) / 2;
  const controlX1 = startX + (midX - startX) * 0.5;
  const controlX2 = endX - (endX - midX) * 0.5;

  return `M ${startX} ${startY} C ${controlX1} ${startY}, ${controlX2} ${endY}, ${endX} ${endY}`;
}

// Get connection label position
getConnectionLabelPosition(connection: FlowConnectionData): { x: number; y: number } {
  const sourceNode = this.flowNodes.find(n => n.id === connection.sourceId);
  const targetNode = this.flowNodes.find(n => n.id === connection.targetId);
  
  if (!sourceNode || !targetNode) {
    return { x: 0, y: 0 };
  }

  const startX = sourceNode.position.x + 180;
  const startY = sourceNode.position.y + 50;
  const endX = targetNode.position.x;
  const endY = targetNode.position.y + 50;

  return {
    x: (startX + endX) / 2,
    y: (startY + endY) / 2 - 10
  };
}

// Updated event handlers for custom flow
onFlowNodeClick(node: FlowNodeData): void {
  console.log('Node clicked:', node);
  this.selectedElement = node.data.node;
  this.selectedFlowNode = node;
  
  // Highlight selected node
  this.highlightFlowNode(node.id);
}

onFlowNodeDoubleClick(node: FlowNodeData): void {
  console.log('Node double clicked:', node);
  
  if (!this.isBasicUser()) {
    this.openModalForEdit = true;
    this.selectedNodeId = node.id;
    this.selectedNode = node.data.node;
    this.selectedFlow = this.flows.find((flow) => flow.id === node.id) || null;
    this.selectedNodeText = node.data.bodyText || '';
  }
}

onFlowConnectionClick(connection: FlowConnectionData): void {
  console.log('Connection clicked:', connection);
  // Add connection selection logic here if needed
}

private highlightFlowNode(nodeId: string): void {
  // Remove previous highlights
  document.querySelectorAll('.flow-node-custom').forEach(node => {
    node.classList.remove('selected');
  });
  
  // Add highlight to selected node
  const selectedNode = document.querySelector(`[data-node-id="${nodeId}"]`);
  if (selectedNode) {
    selectedNode.classList.add('selected');
  }
}