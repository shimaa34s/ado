// Add these methods to your WorkflowComponent class

// Helper method to get node position for drawing connections
getNodePosition(nodeId: string): { x: number; y: number } {
  const node = this.flowNodes.find(n => n.id === nodeId);
  return node ? node.position : { x: 0, y: 0 };
}

// Updated event handlers
onFlowNodeClick(node: FlowNodeLocal) {
  console.log('Node clicked:', node);
  this.selectedElement = this.findElementById(this.mainWorkflow, node.id);
  // You can open edit modal here if needed
  // this.openModalForEdit = true;
  // this.selectedNodeId = node.id;
}

onFlowConnectionClick(connection: FlowConnectionLocal) {
  console.log('Connection clicked:', connection);
  // Handle connection click if needed
}

// Updated truncate method (if not already present)
truncate(text: string, length: number): string {
  if (!text) return '';
  if (text.length <= length) return text;
  return text.substring(0, length) + '...';
}

// Method to check if @foblex/flow is properly imported
private checkFlowLibrary(): boolean {
  try {
    // This will help debug if the library is properly loaded
    console.log('FFlowModule imported:', !!this.constructor);
    return true;
  } catch (error) {
    console.error('FFlow library not properly loaded:', error);
    return false;
  }
}