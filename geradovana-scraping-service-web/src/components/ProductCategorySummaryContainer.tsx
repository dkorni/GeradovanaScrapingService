import { ReactNode } from "react";

interface ProductCategorySummaryContainerPorps {
  children: ReactNode;
}

function ProductCategorySummaryContainer({
  children,
}: ProductCategorySummaryContainerPorps) {
  return (
    <div
      className="row row-cols-1 row-cols-md-1 g-4 flex-grow-1"
      style={{ paddingTop: "44px" }}
    >
      {children}
    </div>
  );
}

export default ProductCategorySummaryContainer;
