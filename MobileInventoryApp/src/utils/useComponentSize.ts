import { useState, useCallback } from "react";

const useComponentSize = () => {
    const [size, setSize] = useState<{width: number, height: number} | null>(null);
  
    const onLayout = useCallback(event => {
      const { width, height }: {width: number, height: number} = event.nativeEvent.layout;
      setSize({ width, height });
    }, []);
  
    return [size, onLayout];
  }

export default useComponentSize