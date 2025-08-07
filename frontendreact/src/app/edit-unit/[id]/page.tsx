import Hero from "@/components/unit/edit/hero";

interface EditUnitProps {
  params: { id: string };
}

export default function EditUnit({ params }: EditUnitProps) {
  console.log("EditUnit params:", params);
  return <Hero id={params.id} />;
}