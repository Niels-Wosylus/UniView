namespace UniView.Utilities
{
    public static class ViewElementExtensions
    {
        public static ViewBase FindParent(this ViewElementBase viewElement)
        {
            var transform = viewElement.transform;                       
            while (transform != null)                            
            {                                                 
                var view = transform.GetComponent<ViewBase>();
                if (view != null && !ReferenceEquals(view, viewElement)) 
                    return view;
                
                transform = transform.parent;                       
            }                                                 
                                                  
            return null; 
        }
    }
}